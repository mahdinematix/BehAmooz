using _01_Framework.Application;
using ServiceHost.AwsServices;
using ServiceHost.AwsServices.AwsDto;

namespace ServiceHost;

public class FileManager : IFileManager
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storageService;
    private static string objName;
    private static string bucketName;
    private static string uploadId;

    public FileManager(IStorageService storageService, IConfiguration configuration)
    {
        _storageService = storageService;
        _configuration = configuration;
    }

    public async Task<string> Upload(IFormFile file, bool isVideo)
    {
        bucketName = _configuration.GetSection("AWS")["BucketName"];
        if (file != null)
        {
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var fileName = Path.GetFileName(file.FileName);
            Random random = new Random();
            var randNumber = random.Next(1, 100);
            objName = $"{randNumber}-{DateTime.Now.ToFileName()}-{fileName}";

            var s3Obj = new S3Object
            {
                InputStream = memoryStream,
                BucketName = bucketName,
                Name = objName
            };

            var cred = new AwsCredentials
            {
                AwsAccessKey = _configuration.GetSection("AWS")["AccessKey"],
                AwsSecretKey = _configuration.GetSection("AWS")["SecretKey"],
                ServiceUrl = _configuration.GetSection("AWS")["ServiceURL"]
            };

            uploadId = await _storageService.UploadFileAsync(s3Obj, cred,isVideo);
            var url = _storageService.GetObject(s3Obj, cred);
            return url;
        }
        return "";
    }

    public async Task Cancel()
    {
        var s3Obj = new S3Object
        {
            BucketName = bucketName,
            Name = objName
        };

        var cred = new AwsCredentials
        {
            AwsAccessKey = _configuration.GetSection("AWS")["AccessKey"],
            AwsSecretKey = _configuration.GetSection("AWS")["SecretKey"],
            ServiceUrl = _configuration.GetSection("AWS")["ServiceURL"]
        };

        await _storageService.AbortMultipartUploadAsync(s3Obj, cred, uploadId);
    }
}