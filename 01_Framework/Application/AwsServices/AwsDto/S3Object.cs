namespace _01_Framework.Application.AwsServices.AwsDto;

public class S3Object
{
    public string Name { get; set; } = null!;
    public MemoryStream InputStream { get; set; } = null!;
    public string BucketName { get; set; } = null!;
}

