namespace _01_Framework.Application.AwsServices.AwsDto
{
    public class UploadResult
    {
        public string BucketName { get; set; }
        public string ObjectName { get; set; }
        public string UploadId { get; set; }
        public UploadStatus UploadStatus { get; set; }
    }
}
