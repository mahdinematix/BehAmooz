using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class PutBucketCors
    {
        // Remember to change the bucket name to the name of an Amazon Simple
        // Storage Service (Amazon S3) bucket that exists on your account.
        private const string BucketName = "<BUCKET_NAME>";

        /// <summary>
        /// The Main method creates the the bucket to be able to accept CORS
        /// requests.
        /// </summary>
        public static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            var _s3Client = new AmazonS3Client(awsCredentials, config);

            CORSConfiguration configuration = new()
            {
                Rules = new List<CORSRule>
              {
                  new CORSRule
                  {
                      Id = "CORSRule1",
                      AllowedMethods = new List<string> { "GET", "PUT", "POST", "DELETE" },
                      AllowedOrigins = new List<string> { "*" },
                      MaxAgeSeconds = 3000,
                      AllowedHeaders = new List<string> { "Authorization" },
                  },
              },
            };

            await PutCORSConfigurationAsync(_s3Client, configuration);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static async Task PutCORSConfigurationAsync(AmazonS3Client client, CORSConfiguration configuration)
        {
            try
            {
                PutCORSConfigurationRequest request = new()
                {
                    BucketName = BucketName,
                    Configuration = configuration,
                };

                PutCORSConfigurationResponse response = await client.PutCORSConfigurationAsync(request);

                Console.WriteLine($"CORS configurations set on {BucketName} bucket");
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
