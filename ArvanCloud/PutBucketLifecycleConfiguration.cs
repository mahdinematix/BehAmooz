using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class PutBucketLifecycleConfiguration
    {
        private const string bucketName = "<BUCKET_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS_KEY>", "<SECRET_KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT_URL>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            PutBucketLifecycle().Wait();
        }
        private static async Task PutBucketLifecycle()
        {
            try
            {
                var lifecycleConfiguration = new LifecycleConfiguration
                {
                    Rules = new List<LifecycleRule>
                  {
                      new LifecycleRule
                      {
                          Filter = new LifecycleFilter
                          {
                              LifecycleFilterPredicate = new LifecyclePrefixPredicate
                              {
                                  Prefix = "someprefix/"
                              }
                          },
                          Expiration = new LifecycleRuleExpiration  { Days = 10 },
                          Status = "Enabled",
                      }
                  }
                };

                var putLifecycleConfigurationRequest = new PutLifecycleConfigurationRequest
                {
                    BucketName = bucketName,
                    Configuration = lifecycleConfiguration
                };

                await _s3Client.PutLifecycleConfigurationAsync(putLifecycleConfigurationRequest);

                Console.WriteLine($"Lifecycle configuration added to {bucketName} bucket");
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
