using Amazon.S3;
using Amazon.S3.Util;

namespace ArvanCloud
{
    public class HeadBucket
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

            await HeadBucketAsync(_s3Client, BUCKET_NAME);
        }

        /// <summary>
        /// HeadBucketAsync calls the DoesS3BucketExist method
        /// to check if the S3 bucket bucketName exists.
        /// </summary>
        /// <param name="client">The Amazon S3 client object.</param>
        /// <param name="bucketName">The name of the bucket to check.</param>
        static async Task HeadBucketAsync(IAmazonS3 client, string bucketName)
        {
            bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(client, bucketName);

            if (bucketExists)
            {
                Console.WriteLine("Bucket Exists");
            }
            else
            {
                Console.WriteLine("Bucket DOES NOT Exists");
            }

            Console.WriteLine(bucketExists);
        }
    }
}
