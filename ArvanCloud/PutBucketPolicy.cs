using Amazon.S3;
using Amazon.S3.Model;
using System.Reflection;

namespace ArvanCloud
{
    public class PutBucketPolicy
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

            await PutBucketPolicyAsync(_s3Client, BUCKET_NAME);
        }

        /// <summary>
        /// PutBucketPolicyAsync calls the DoesS3BucketExistV2Async method
        /// to set bucket's policies.
        /// </summary>
        /// <param name="client">The Amazon S3 client object.</param>
        /// <param name="bucketName">The name of the bucket to check.</param>
        static async Task PutBucketPolicyAsync(IAmazonS3 client, string bucketName)
        {
            string newPolicy = @"{
              ""Statement"":[{
              ""Sid"":""PolicyName"",
              ""Effect"":""Allow"",
              ""Principal"": { ""AWS"": ""*"" },
              ""Action"":[""s3:PutObject"",""s3:GetObject""],
              ""Resource"":[""arn:aws:s3:::rezvani/user_*""]
          }]}";

            PutBucketPolicyRequest putRequest = new PutBucketPolicyRequest
            {
                BucketName = bucketName,
                Policy = newPolicy
            };

            Object policy = await client.PutBucketPolicyAsync(putRequest);

            foreach (PropertyInfo prop in policy.GetType().GetProperties())
            {
                Console.WriteLine($"{prop.Name}: {prop.GetValue(policy, null)}");
            }

            Console.WriteLine($"Policy successfully added to {bucketName} bucket");
        }
    }
}
