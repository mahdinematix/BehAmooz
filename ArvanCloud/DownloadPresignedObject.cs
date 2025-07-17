using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class DownloadPresignedObject
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "<OBJECT_NAME>";
        private const string OBJECT_NAME = "<BUCKET_NAME>";

        static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            GeneratePreSignedUrl();
        }

        private static void GeneratePreSignedUrl()
        {
            try
            {
                // Create the request
                var getPreSignedUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = BUCKET_NAME,
                    Key = OBJECT_NAME,
                    Expires = DateTime.Now.AddHours(1.0),
                    Verb = HttpVerb.GET
                };

                // Submit the request
                string url = _s3Client.GetPreSignedURL(getPreSignedUrlRequest);
                Console.WriteLine($"URL: {url}");
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
