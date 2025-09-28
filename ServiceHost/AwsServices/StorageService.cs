using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.SignalR;
using ServiceHost.AwsServices.AwsDto;
using ServiceHost.Hubs;
using S3Object = ServiceHost.AwsServices.AwsDto.S3Object;

namespace ServiceHost.AwsServices;

public class StorageService : IStorageService
{
    private readonly IHubContext<UploadHub> _hubContext;

    public StorageService(IHubContext<UploadHub> hubContext)
    {
        _hubContext = hubContext;
    }

    private static IAmazonS3 _s3Client;
    private static string uploadId;

    public async Task UploadFileAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo)
    {
        var credentials = new BasicAWSCredentials(awsCredentials.AwsAccessKey, awsCredentials.AwsSecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = awsCredentials.ServiceUrl
        };
        _s3Client = new AmazonS3Client(credentials, config);

        List<UploadPartResponse> uploadResponses = new();
        InitiateMultipartUploadRequest initiateRequest = new()
        {
            BucketName = s3Object.BucketName,
            Key = s3Object.Name,
        };
        InitiateMultipartUploadResponse initResponse =
            await _s3Client.InitiateMultipartUploadAsync(initiateRequest);
        uploadId = initResponse.UploadId;
        long contentLength = s3Object.InputStream.Length;
        const int partSize = 5 * 1024 * 1024;

        try
        {
            long numOfParts = contentLength / partSize;
            long filePosition = 0;
            for (int i = 1; filePosition < contentLength; i++)
            {
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
                uploadResponses.Add(await _s3Client.UploadPartAsync(uploadPartRequest));
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
            await _s3Client.CompleteMultipartUploadAsync(completeRequest);

            if (isVideo)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveVideoProgress", 100);
            }
            else
            {
                await _hubContext.Clients.All.SendAsync("ReceiveBookletProgress", 100);
            }
        }
        catch (Exception ex)
        {
            var abortRequest = new AbortMultipartUploadRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId
            };
            await _s3Client.AbortMultipartUploadAsync(abortRequest);
        }
    }



    public string GetObject(S3Object s3Object, AwsCredentials awsCredentials)
    {
        var credentials = new BasicAWSCredentials(awsCredentials.AwsAccessKey, awsCredentials.AwsSecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = awsCredentials.ServiceUrl
        };
        _s3Client = new AmazonS3Client(credentials, config);

        try
        {
            var getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                Expires = DateTime.Now.AddYears(20),
                Verb = HttpVerb.GET
            };

            string url = _s3Client.GetPreSignedURL(getPreSignedUrlRequest);
            return url;
        }
        catch (Exception e)
        {
            return "";
        }
    }

    public async Task AbortMultipartUploadAsync(S3Object s3Object, AwsCredentials awsCredentials)
    {
        var credentials = new BasicAWSCredentials(awsCredentials.AwsAccessKey, awsCredentials.AwsSecretKey);
        var config = new AmazonS3Config
        {
            ServiceURL = awsCredentials.ServiceUrl
        };
        _s3Client = new AmazonS3Client(credentials, config);

        try
        {
            AbortMultipartUploadRequest request = new()
            {
                BucketName = s3Object.BucketName,
                Key = s3Object.Name,
                UploadId = uploadId
            };
            
            await _s3Client.AbortMultipartUploadAsync(request);

        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.ToString());
        }
    }
}