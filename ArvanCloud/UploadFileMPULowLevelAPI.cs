using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace ArvanCloud
{
    public class UploadFileMPULowLevelAPI
    {
        private static IAmazonS3 _s3Client;

        private const string BUCKET_NAME = "behamooz-bucket";
        private static string LOCAL_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private const string OBJECT_NAME = "test2";

        public static async Task Main()
        {
            string bucketName = BUCKET_NAME;
            string keyName = OBJECT_NAME;
            string filePath = $"{LOCAL_PATH}/{keyName}";

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials("<ACCESS-KEY>", "<SECRET-KEY>");
            var config = new AmazonS3Config { ServiceURL = "<ENDPOINT>" };
            _s3Client = new AmazonS3Client(awsCredentials, config);

            Console.WriteLine("Uploading an object...");
            await UploadObjectAsync(_s3Client, bucketName, keyName, filePath);
        }

        /// <summary>
        /// Uses the low-level API to upload an object from the local system to
        /// to an S3 bucket.
        /// </summary>
        /// <param name="client">The initialized S3 client object used to
        /// perform the multi-part upload.</param>
        /// <param name="bucketName">>The name of the bucket to which to upload
        /// the file.</param>
        /// <param name="keyName">The file name to be used in the
        /// destination S3 bucket.</param>
        /// <param name="filePath">The path, including the file name of the
        /// file to be uploaded to the S3 bucket.</param>
        public static async Task UploadObjectAsync(
            IAmazonS3 client,
            string bucketName,
            string keyName,
            string filePath)
        {
            // Create list to store upload part responses.
            List<UploadPartResponse> uploadResponses = new();

            // Setup information required to initiate the multipart upload.
            InitiateMultipartUploadRequest initiateRequest = new()
            {
                BucketName = bucketName,
                Key = keyName,
            };

            // Initiate the upload.
            InitiateMultipartUploadResponse initResponse =
                await client.InitiateMultipartUploadAsync(initiateRequest);

            // Upload parts.
            long contentLength = new FileInfo(filePath).Length;
            long partSize = 400 * (long)Math.Pow(2, 20); // 400 MB

            try
            {
                Console.WriteLine("Uploading parts");

                long filePosition = 0;
                for (int i = 1; filePosition < contentLength; i++)
                {
                    UploadPartRequest uploadRequest = new()
                    {
                        BucketName = bucketName,
                        Key = keyName,
                        UploadId = initResponse.UploadId,
                        PartNumber = i,
                        PartSize = partSize,
                        FilePosition = filePosition,
                        FilePath = filePath,
                    };

                    // Track upload progress.
                    uploadRequest.StreamTransferProgress +=
                        new EventHandler<StreamTransferProgressArgs>(UploadPartProgressEventCallback);

                    // Upload a part and add the response to our list.
                    uploadResponses.Add(await client.UploadPartAsync(uploadRequest));

                    filePosition += partSize;
                }

                // Setup to complete the upload.
                CompleteMultipartUploadRequest completeRequest = new()
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId,
                };
                completeRequest.AddPartETags(uploadResponses);

                // Complete the upload.
                CompleteMultipartUploadResponse completeUploadResponse =
                    await client.CompleteMultipartUploadAsync(completeRequest);

                Console.WriteLine($"Object {keyName} added to {bucketName} bucket");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An AmazonS3Exception was thrown: {exception.Message}");

                // Abort the upload.
                AbortMultipartUploadRequest abortMPURequest = new()
                {
                    BucketName = bucketName,
                    Key = keyName,
                    UploadId = initResponse.UploadId,
                };
                await client.AbortMultipartUploadAsync(abortMPURequest);
            }
        }

        /// <summary>
        /// Handles the UploadProgress even to display the progress of the
        /// S3 multi-part upload.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">The event parameters.</param>
        public static void UploadPartProgressEventCallback(object sender, StreamTransferProgressArgs e)
        {
            Console.WriteLine($"{e.TransferredBytes}/{e.TotalBytes}");
        }
    }
}
