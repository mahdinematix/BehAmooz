using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class PutObjectAcl
    {
        private const string bucketName = "<BUCKET_NAME>";
        private const string objectName = "<OBJECT_NAME>";
        private const string acl = "private"; // private or public-read
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            PutObjectAclAsync().Wait();
        }
        private static async Task PutObjectAclAsync()
        {
            try
            {
                // Set a new ACL.
                PutACLResponse response = await _s3Client.PutACLAsync(new PutACLRequest
                {
                    BucketName = bucketName,
                    Key = objectName,
                    CannedACL = acl == "private" ? S3CannedACL.Private : S3CannedACL.PublicRead, // S3CannedACL.PublicRead or S3CannedACL.Private
                });

                Console.WriteLine($"Access-list {acl} added to {objectName} object");
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
