using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

namespace ArvanCloud
{
    public class CreateBucket
    {

        private static IAmazonS3 _s3Client;

        static async Task Main()
        {
            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("60658be5-4555-4f5c-9e86-7a1fc18407cf", "3615734d947b09c070d965b4c3986af995fda0572dafd6f0b1df6fd32e60ccf3");
            var config = new AmazonS3Config { ServiceURL = "s3.ir-thr-at1.arvanstorage.ir" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            
        }
    }
}