using _01_Framework.Application.AwsServices.AwsDto;
using _01_Framework.Hubs;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.SignalR;
using S3Object = _01_Framework.Application.AwsServices.AwsDto.S3Object;

namespace _01_Framework.Application.AwsServices;

public class StorageService : IStorageService
{
    private readonly IHubContext<UploadHub> _hubContext;

    public StorageService(IHubContext<UploadHub> hubContext)
    {
        _hubContext = hubContext;
    }

    private IAmazonS3 CreateS3Client(AwsCredentials awsCredentials)
    {
        var credentials = new BasicAWSCredentials(awsCredentials.AwsAccessKey, awsCredentials.AwsSecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = awsCredentials.ServiceUrl
        };
        return new AmazonS3Client(credentials, config);
    }


    public async Task<string> InitiateUploadAsync(S3Object s3Object, AwsCredentials awsCredentials)
    {
        using var s3Client = CreateS3Client(awsCredentials);
        var request = new InitiateMultipartUploadRequest
        {
            BucketName = s3Object.BucketName,
            Key = s3Object.Name
        };
        var response = await s3Client.InitiateMultipartUploadAsync(request);
        return response.UploadId;
    }

    public string GetObject(S3Object s3Object, AwsCredentials awsCredentials)
    {
        using var s3Client = CreateS3Client(awsCredentials);
        try
        {

            try
            {
                var metadata = s3Client.GetObjectMetadataAsync(s3Object.BucketName, s3Object.Name).Result;
            }
            catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return "";
            }

            var getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                Expires = DateTime.Now.AddYears(20),
                Verb = HttpVerb.GET
            };

            string url = s3Client.GetPreSignedURL(getPreSignedUrlRequest);

            if (string.IsNullOrEmpty(url) || url.Contains("<Error>") || url.Contains("NoSuchKey"))
                return "";
            return url;
        }
        catch (Exception e)
        {
            return "";
        }
    }

    public async Task<string> UploadPartsAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo, CancellationToken token, string uploadId)
    {
        using var s3Client = CreateS3Client(awsCredentials);
        List<UploadPartResponse> uploadResponses = new();

        long contentLength = s3Object.InputStream.Length;
        const int partSize = 5 * 1024 * 1024;

        try
        {
            long numOfParts = contentLength / partSize;
            long filePosition = 0;
            for (int i = 1; filePosition < contentLength; i++)
            {
                token.ThrowIfCancellationRequested();

                s3Object.InputStream.Position = filePosition;
                var uploadPartRequest = new UploadPartRequest
                {
                    BucketName = s3Object.BucketName,
                    Key = s3Object.Name,
                    UploadId = uploadId,
                    PartNumber = i,
                    PartSize = partSize,
                    FilePosition = filePosition,
                    InputStream = s3Object.InputStream
                };
                uploadResponses.Add(await s3Client.UploadPartAsync(uploadPartRequest, token));
                filePosition += partSize;
                if (numOfParts > 0)
                {
                    long percent = (i * 100) / numOfParts;
                    if (isVideo)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveVideoProgress", percent);
                    }
                    else
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveBookletProgress", percent);
                    }
                }
            }

            token.ThrowIfCancellationRequested();

            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId,
            };

            completeRequest.AddPartETags(uploadResponses);
            await s3Client.CompleteMultipartUploadAsync(completeRequest, token);

            if (isVideo)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveVideoProgress", 100);
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("ReceiveBookletProgress", 100);
            }

            return uploadId;
        }
        catch (Exception ex)
        {
            await AbortUploadAsync(s3Object, awsCredentials, uploadId);

            using var s3Client2 = CreateS3Client(awsCredentials);
            await s3Client2.DeleteObjectAsync(s3Object.BucketName, s3Object.Name);


            Console.WriteLine($"Upload aborted due to: {ex.Message}");
            return "";
        }

        finally
        {
            s3Object.InputStream?.Dispose();
        }
    }

    public async Task AbortUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId)
    {
        using var s3Client = CreateS3Client(awsCredentials);

        try
        {
            var request = new AbortMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId
            };

            await s3Client.AbortMultipartUploadAsync(request);

            var deleteRequest = new DeleteObjectsRequest
            {
                BucketName = s3Object.BucketName,
                Objects = new List<KeyVersion> { new() { Key = s3Object.Name } }
            };

            await s3Client.DeleteObjectsAsync(deleteRequest);

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }
}