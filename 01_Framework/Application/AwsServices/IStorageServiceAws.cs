using _01_Framework.Application.AwsServices.AwsDto;

namespace _01_Framework.Application.AwsServices;

public interface IStorageServiceAws
{

    Task<string> InitiateUploadAsync(S3Object s3Object, AwsCredentials awsCredentials);
    Task<string> GetObject(S3Object s3Object, AwsCredentials awsCredentials);
    Task<string> UploadPartsAsync(S3Object s3Object, AwsCredentials awsCredentials,
        CancellationToken token, string uploadId);
    Task AbortUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId);

}

