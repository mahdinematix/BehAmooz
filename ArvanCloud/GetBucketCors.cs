using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class GetBucketCors
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
            await RetrieveCORSConfigurationAsync(_s3Client);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        private static async Task<CORSConfiguration> RetrieveCORSConfigurationAsync(AmazonS3Client client)
        {
            GetCORSConfigurationRequest request = new()
            {
                BucketName = BucketName,
            };
            var response = await client.GetCORSConfigurationAsync(request);
            var configuration = response.Configuration;
            PrintCORSRules(configuration);
            return configuration;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        private static void PrintCORSRules(CORSConfiguration configuration)
        {
            Console.WriteLine();

            if (configuration is null)
            {
                Console.WriteLine("\nConfiguration is null");
                return;
            }

            Console.WriteLine($"Configuration has {configuration.Rules.Count} rules:");
            foreach (CORSRule rule in configuration.Rules)
            {
                Console.WriteLine($"Rule ID: {rule.Id}");
                Console.WriteLine($"MaxAgeSeconds: {rule.MaxAgeSeconds}");
                Console.WriteLine($"AllowedMethod: {string.Join(", ", rule.AllowedMethods.ToArray())}");
                Console.WriteLine($"AllowedOrigins: {string.Join(", ", rule.AllowedOrigins.ToArray())}");
                Console.WriteLine($"AllowedHeaders: {string.Join(", ", rule.AllowedHeaders.ToArray())}");
                Console.WriteLine($"ExposeHeader: {string.Join(", ", rule.ExposeHeaders.ToArray())}");
            }
        }
    }
}
