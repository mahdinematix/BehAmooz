using _01_Framework.Application.AwsServices.AwsDto;

namespace _01_Framework.Application.AwsServices;

public interface IStorageService
{
    Task<string> UploadFileAsync(S3Object s3Object, AwsCredentials awsCredentials, bool isVideo, CancellationToken cancellationToken);
    string GetObject(S3Object s3Object, AwsCredentials awsCredentials);
    Task AbortMultipartUploadAsync(S3Object s3Object, AwsCredentials awsCredentials, string uploadId);
}

