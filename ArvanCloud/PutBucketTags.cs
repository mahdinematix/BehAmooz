using Amazon.S3;
using Amazon.S3.Model;
using System.Reflection;

namespace ArvanCloud
{
    public class PutBucketTags
    {
        private const string bucketName = "<BUCKET_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            PutBucketTagsAsync().Wait();
        }

        private static async Task PutBucketTagsAsync()
        {
            try
            {
                List<Tag> tagList = new List<Tag>();
                // Add parts to the list.
                tagList.Add(new Tag() { Key = "Key1", Value = "Value1" });
                tagList.Add(new Tag() { Key = "Key2", Value = "Value2" });

                PutBucketTaggingResponse response = await _s3Client.PutBucketTaggingAsync(bucketName, tagList);

                foreach (PropertyInfo prop in response.GetType().GetProperties())
                {
                    Console.WriteLine($"{prop.Name}: {prop.GetValue(response, null)}");
                }

                Console.WriteLine($"Tags added to {bucketName} bucket");
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
