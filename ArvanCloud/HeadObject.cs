using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class HeadObject
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<BUCKET_NAME>";
        private const string OBJECT_NAME = "<OBJECT_NAME>";

        static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            HeadObjectHelper().Wait();
        }

        private static async Task HeadObjectHelper()
        {
            try
            {
                // Create the request
                GetObjectMetadataRequest request = new()
                {
                    BucketName = BUCKET_NAME,
                    Key = OBJECT_NAME
                };

                // Submit the request
                GetObjectMetadataResponse response = await _s3Client.GetObjectMetadataAsync(request);

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
