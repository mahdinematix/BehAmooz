using ServiceHost.AwsServices.AwsDto;

namespace ServiceHost.AwsServices;

public interface IStorageService
{
    Task<string> UploadFileAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo);
    string GetObject(S3Object s3Object, AwsCredentials awsCredentials);
    Task AbortMultipartUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId);
}

