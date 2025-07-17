using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class GetBucketLifecycleConfiguration
    {
        private const string bucketName = "<BUCKET_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS_KEY>", "<SECRET_KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT_URL>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            GetBucketLifecycle().Wait();
        }
        private static async Task GetBucketLifecycle()
        {
            try
            {
                GetLifecycleConfigurationResponse response = await _s3Client.GetLifecycleConfigurationAsync(new GetLifecycleConfigurationRequest
                {
                    BucketName = bucketName,
                });

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
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
