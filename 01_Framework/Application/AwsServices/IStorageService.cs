using _01_Framework.Application.AwsServices.AwsDto;

namespace _01_Framework.Application.AwsServices;

public interface IStorageService
{

    Task<string> InitiateUploadAsync(S3Object s3Object, AwsCredentials awsCredentials);
    string GetObject(S3Object s3Object, AwsCredentials awsCredentials);
    Task<string> UploadPartsAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo,
        CancellationToken token, string uploadId);
    Task AbortUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId);

}

