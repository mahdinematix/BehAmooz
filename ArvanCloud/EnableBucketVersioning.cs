using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class EnableBucketVersioning
    {
        private const string bucketName = "<BUCKET_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            EnableBucketVersioningAsync().Wait();
        }
        private static async Task EnableBucketVersioningAsync()
        {
            try
            {
                PutBucketVersioningResponse response = await _s3Client.PutBucketVersioningAsync(new PutBucketVersioningRequest
                {
                    BucketName = bucketName,
                    VersioningConfig = new S3BucketVersioningConfig() { Status = VersionStatus.Enabled }
                });

                Console.WriteLine($"Versioning enabled in {bucketName} bucket");
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
