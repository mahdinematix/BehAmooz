using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class PutObjectTags
    {
        private const string bucketName = "<BUCKET_NAME>";
        private const string objectName = "<OBJECT_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            PutObjectTagsAsync().Wait();
        }
        private static async Task PutObjectTagsAsync()
        {
            try
            {
                var tagging = new Tagging
                {
                    TagSet = new List<Tag>
                    {
                        new Tag
                        {
                            Key = "key1",
                            Value = "value1"
                        }

                    }
                };
                // Set a new Tag set for object.
                PutObjectTaggingResponse response = await _s3Client.PutObjectTaggingAsync(new PutObjectTaggingRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    Tagging = tagging
                });

                Console.WriteLine($"Tag set added to {objectName} object");
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
