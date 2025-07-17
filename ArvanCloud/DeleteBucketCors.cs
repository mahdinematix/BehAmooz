using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class DeleteBucketCors
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<BUCKET_NAME>";

        static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            DeleteBucketCorsAsync().Wait();
        }

        private static async Task DeleteBucketCorsAsync()
        {
            try
            {
                // Create the request
                DeleteCORSConfigurationRequest request = new()
                {
                    BucketName = BUCKET_NAME,
                };

                // Submit the request
                DeleteCORSConfigurationResponse response = await _s3Client.DeleteCORSConfigurationAsync(request);

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
                Console.WriteLine($"CORS configuration successfully deleted from {BUCKET_NAME} bucket");
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
