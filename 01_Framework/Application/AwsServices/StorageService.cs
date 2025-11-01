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

    public async Task<string> UploadFileAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo, CancellationToken cancellationToken)
    {
        using var s3Client = CreateS3Client(awsCredentials);

        List<UploadPartResponse> uploadResponses = new();
        InitiateMultipartUploadRequest initiateRequest = new()
        {
            BucketName = s3Object.BucketName,
            Key = s3Object.Name,
        };
        var initResponse =
            await s3Client.InitiateMultipartUploadAsync(new InitiateMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name
            }, cancellationToken);
        var uploadId = initResponse.UploadId;
        long contentLength = s3Object.InputStream.Length;
        const int partSize = 5 * 1024 * 1024;

        try
        {
            long numOfParts = contentLength / partSize;
            long filePosition = 0;
            for (int i = 1; filePosition < contentLength; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

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
                uploadResponses.Add(await s3Client.UploadPartAsync(uploadPartRequest, cancellationToken));
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

            var completeRequest = new CompleteMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId,
            };

            completeRequest.AddPartETags(uploadResponses);
            await s3Client.CompleteMultipartUploadAsync(completeRequest, cancellationToken);

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
            var abortRequest = new AbortMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId
            };
            await s3Client.AbortMultipartUploadAsync(abortRequest);
            return "";
        }
    }



    public string GetObject(S3Object s3Object, AwsCredentials awsCredentials)
    {
        using var s3Client = CreateS3Client(awsCredentials);

        try
        {
            var getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                Expires = DateTime.Now.AddYears(20),
                Verb = HttpVerb.GET
            };

            string url = s3Client.GetPreSignedURL(getPreSignedUrlRequest);
            return url;
        }
        catch (Exception e)
        {
            return "";
        }
    }

    public async Task AbortMultipartUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId)
    {
        using var s3Client = CreateS3Client(awsCredentials);

        try
        {
            AbortMultipartUploadRequest request = new()
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId
            };
            
            await s3Client.AbortMultipartUploadAsync(request);

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }
}