using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class DeleteBucketLifecycleConfiguration
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<BUCKET_NAME>";

        static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS_KEY>", "<SECRET_KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT_URL>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            DeleteBucketLifecycle().Wait();
        }

        private static async Task DeleteBucketLifecycle()
        {
            try
            {
                // Create the request
                DeleteLifecycleConfigurationRequest request = new()
                {
                    BucketName = BUCKET_NAME,
                };

                // Submit the request
                DeleteLifecycleConfigurationResponse response = await _s3Client.DeleteLifecycleConfigurationAsync(request);

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
                Console.WriteLine($"Lifecycle configuration successfully deleted from {BUCKET_NAME} bucket");
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
