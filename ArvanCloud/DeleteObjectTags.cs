using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class DeleteObjectTags
    {
        private const string bucketName = "<BUCKET_NAME>";
        private const string objectName = "<OBJECT_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            DeleteObjectTagsAsync().Wait();
        }
        private static async Task DeleteObjectTagsAsync()
        {
            try
            {
                DeleteObjectTaggingResponse response = await _s3Client.DeleteObjectTaggingAsync(new DeleteObjectTaggingRequest
                {
                    BucketName = bucketName,
                    Key = objectName
                });

                Console.WriteLine(JsonConvert.SerializeObject(response, Formatting.Indented));
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
