﻿using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;

namespace ArvanCloud
{
    public class GetBucketAcl
    {
        private const string bucketName = "<BUCKET_NAME>";
        private static IAmazonS3 _s3Client;
        public static void Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);
            GetBucketAclAsync().Wait();
        }
        private static async Task GetBucketAclAsync()
        {
            try
            {
                GetACLResponse response = await _s3Client.GetACLAsync(new GetACLRequest
                {
                    BucketName = bucketName,
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
