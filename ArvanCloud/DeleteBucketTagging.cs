using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class DeleteBucketTagging
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<BUCKET_NAME>";

        static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            DeleteBucketTaggingAsync().Wait();
        }

        private static async Task DeleteBucketTaggingAsync()
        {
            try
            {
                // Create the request
                DeleteBucketTaggingRequest request = new()
                {
                    BucketName = BUCKET_NAME,
                };

                // Submit the request
                DeleteBucketTaggingResponse response = await _s3Client.DeleteBucketTaggingAsync(request);

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
                Console.WriteLine($"Bucket tagging successfully deleted from {BUCKET_NAME} bucket");
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
