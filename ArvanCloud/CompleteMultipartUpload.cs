using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class CompleteMultipartUpload
    {
        private const string bucketName = "<BUCKET_NAME>";
        private const string objectName = "<OBJECT_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            CompleteMultipartUploadAsync().Wait();
        }
        private static async Task CompleteMultipartUploadAsync()
        {
            try
            {
                CompleteMultipartUploadRequest compRequest = new CompleteMultipartUploadRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    UploadId = "123",
                    PartETags = new List<PartETag>
                    {
                        new PartETag { ETag = "string", PartNumber = 123 },
                    }
                };

                CompleteMultipartUploadResponse compResponse = await _s3Client.CompleteMultipartUploadAsync(compRequest);

                Console.WriteLine($"Multipart upload completed");
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                Console.WriteLine("An AmazonS3Exception was thrown. Exception: " + amazonS3Exception.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }
    }
}
