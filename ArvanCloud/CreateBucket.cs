using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class CreateBucket
    {
        private static IAmazonS3 _s3Client;

        static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            Console.WriteLine($"\nCreating a new bucket, named: <NEW_BUCKET_NAME>.");

            await CreatingBucketAsync(_s3Client, "<NEW_BUCKET_NAME>");
        }


        /// <summary>
        /// Uses Amazon SDK for .NET PutBucketAsync to create a new
        /// Amazon S3 bucket.
        /// </summary>
        /// <param name="client">The client object used to connect to Amazon S3.</param>
        /// <param name="bucketName">The name of the bucket to create.</param>
        static async Task CreatingBucketAsync(IAmazonS3 client, string bucketName)
        {
            try
            {
                var putBucketRequest = new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                };

                var putBucketResponse = await client.PutBucketAsync(putBucketRequest);

            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error creating bucket: '{ex.Message}'");
            }
        }
    }
}