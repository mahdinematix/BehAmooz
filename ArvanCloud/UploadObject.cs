using Amazon.S3;
using Amazon.S3.Model;
using System.Reflection;

namespace ArvanCloud
{
    class UploadObject
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<BUCKET_NAME>";
        private const string OBJECT_NAME = "<OBJECT_NAME>";

        private static string LOCAL_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            // The method expects the full path, including the file name.
            var path = $"{LOCAL_PATH}/{OBJECT_NAME}";

            await UploadObjectFromFileAsync(_s3Client, BUCKET_NAME, OBJECT_NAME, path);
        }

        /// <summary>
        /// This method uploads a file to an Amazon S3 bucket. This
        /// example method also adds metadata for the uploaded file.
        /// </summary>
        /// <param name="client">An initialized Amazon S3 client object.</param>
        /// <param name="bucketName">The name of the S3 bucket to upload the
        /// file to.</param>
        /// <param name="objectName">The destination file name.</param>
        /// <param name="filePath">The full path, including file name, to the
        /// file to upload. This doesn't necessarily have to be the same as the
        /// name of the destination file.</param>
        private static async Task UploadObjectFromFileAsync(
            IAmazonS3 client,
            string bucketName,
            string objectName,
            string filePath)
        {
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    FilePath = filePath,
                    ContentType = "text/plain"
                };

                putRequest.Metadata.Add("x-amz-meta-title", "someTitle");

                PutObjectResponse response = await client.PutObjectAsync(putRequest);

                foreach (PropertyInfo prop in response.GetType().GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(response, null)}");
                }

                Console.WriteLine($"Object {OBJECT_NAME} added to {bucketName} bucket");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
