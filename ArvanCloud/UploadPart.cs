using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class UploadPart
    {
        private const string bucketName = "<BUCKET_NAME>";
        private const string objectName = "<OBJECT_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            UploadPartAsync().Wait();
        }
        private static async Task UploadPartAsync()
        {
            try
            {
                UploadPartRequest uploadRequest = new UploadPartRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    UploadId = "123",
                    PartNumber = 1,
                };

                UploadPartResponse up1Response = await _s3Client.UploadPartAsync(uploadRequest);

                Console.WriteLine($"Upload part completed");
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
