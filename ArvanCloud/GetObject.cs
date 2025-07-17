using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class GetObject
    {
        private const string OBJECT_NAME = "<OBJECT_NAME>";

        public static async Task Main()
        {
            const string bucketName = "<BUCKET_NAME>";
            var keyName = OBJECT_NAME;

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            IAmazonS3 _s3Client = new AmazonS3Client(awsCredentials, config);
            await ReadObjectDataAsync(_s3Client, bucketName, keyName);
        }

        /// <summary>
        /// This method copies the contents of the object keyName to another
        /// location, for example, to your local system.
        /// </summary>
        /// <param name="client">The initialize S3 client used to call
        /// GetObjectAsync.</param>
        /// <param name="bucketName">The name of the S3 bucket which contains
        /// the object to copy.</param>
        /// <param name="keyName">The name of the object you want to copy.</param>
        static async Task ReadObjectDataAsync(IAmazonS3 client, string bucketName, string keyName)
        {
            string responseBody = string.Empty;

            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    // Assume you have "title" as medata added to the object.
                    string title = response.Metadata["x-amz-meta-title"];
                    string contentType = response.Headers["Content-Type"];

                    Console.WriteLine($"Object metadata, Title: {title}");
                    Console.WriteLine($"Content type: {contentType}");

                    // Retrieve the contents of the file.
                    responseBody = reader.ReadToEnd();

                    // Write the contents of the file to disk.
                    string filePath = keyName;

                    Console.WriteLine("File successfully downloaded");
                }
            }
            catch (AmazonS3Exception e)
            {
                // If the bucket or the object do not exist
                Console.WriteLine($"Error: '{e.Message}'");
            }
        }
    }
}
