using Amazon.S3;
using Amazon.S3.Model;
using System.Reflection;

namespace ArvanCloud
{
    public class GetBucketPolicy
    {
        // This example shows how to check a bucket existence.
        //  The examples uses AWS SDK for .NET 3.5 and .NET 5.0.

        private static IAmazonS3 _s3Client;

        // Specify the name of the bucket to check.
        private const string BUCKET_NAME = "<BUCKET_NAME>";

        static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            await GetBucketPolicyAsync(_s3Client, BUCKET_NAME);
        }

        /// <summary>
        /// GetBucketPolicyAsync calls the DoesS3BucketExistV2Async method
        /// to get bucket's policies.
        /// </summary>
        /// <param name="client">The Amazon S3 client object.</param>
        /// <param name="bucketName">The name of the bucket to check.</param>
        static async Task GetBucketPolicyAsync(IAmazonS3 client, string bucketName)
        {
            GetBucketPolicyRequest getRequest = new GetBucketPolicyRequest
            {
                BucketName = bucketName
            };
            Object policy = await client.GetBucketPolicyAsync(getRequest);

            foreach (PropertyInfo prop in policy.GetType().GetProperties())
            {
                Console.WriteLine($"{prop.Name}: {prop.GetValue(policy, null)}");
            }
        }
    }
}
