using Amazon.S3;

namespace ArvanCloud
{
    public class DeleteBucket
    {
        // This example shows how to delete an existing empty bucket.
        //  The examples uses AWS SDK for .NET 3.5 and .NET 5.0.

        private static IAmazonS3 _s3Client;

        // Specify the name of the bucket to delete.
        private const string BUCKET_NAME = "<BUCKET_NAME>";

        static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            // Now delete the bucket. If the bucket you are trying to
            // delete contains any objects, the call will raise an exception.
            Console.WriteLine($"\nDeleting bucket {BUCKET_NAME}...");
            await DeletingBucketAsync(_s3Client, BUCKET_NAME);
        }

        /// <summary>
        /// DeletingBucketAsync calls the DeleteBucketAsync method
        /// to delete the S3 bucket bucketName.
        /// </summary>
        /// <param name="client">The Amazon S3 client object.</param>
        /// <param name="bucketName">The name of the bucket to be deleted.</param>
        static async Task DeletingBucketAsync(IAmazonS3 client, string bucketName)
        {
            try
            {
                var deleteResponse = await client.DeleteBucketAsync(bucketName);
                Console.WriteLine($"\nResult: {deleteResponse.HttpStatusCode.ToString()}");
                Console.WriteLine("Bucket successfully deleted");
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
