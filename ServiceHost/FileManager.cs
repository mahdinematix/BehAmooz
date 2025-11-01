using _01_Framework.Application;
using _01_Framework.Application.AwsServices;
using _01_Framework.Application.AwsServices.AwsDto;
using Microsoft.Extensions.Caching.Memory;

namespace ServiceHost;

public class FileManager : IFileManager
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storageService;
    private readonly IMemoryCache _cache;
    private readonly IAuthHelper _authHelper;

    public FileManager(IStorageService storageService, IConfiguration configuration, IMemoryCache cache, IAuthHelper authHelper)
    {
        _storageService = storageService;
        _configuration = configuration;
        _cache = cache;
        _authHelper = authHelper;
    }

    private string GetUserCacheKey()
    {
        var userId = _authHelper.CurrentAccountId();
        return $"upload_user_{userId}";
    }
    private readonly Dictionary<string, CancellationTokenSource> _uploadTokens = new();


    public async Task<string> Upload(IFormFile file, bool isVideo)
    {
        if (file == null) return "";

        var userKey = GetUserCacheKey();
        _cache.Remove(userKey);
        if (_uploadTokens.TryGetValue(userKey, out var oldCts))
        {
            oldCts.Cancel();
            _uploadTokens.Remove(userKey);
        }

        var cts = new CancellationTokenSource();
        _uploadTokens[userKey] = cts;

        var bucketName = _configuration.GetSection("AWS")["BucketName"];
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var fileName = Path.GetFileName(file.FileName);
        var objName = $"{new Random().Next(1, 100)}-{DateTime.Now:yyyyMMddHHmmss}-{fileName}";

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

        var uploadId = await _storageService.UploadFileAsync(s3Obj, cred, isVideo, cts.Token);

        _uploadTokens.Remove(userKey);

        if (!string.IsNullOrEmpty(uploadId))
        {
            _cache.Set(userKey, new UploadResult
            {
                BucketName = bucketName,
                ObjectName = objName,
                UploadId = uploadId
            }, TimeSpan.FromMinutes(30));
        }

        return _storageService.GetObject(s3Obj, cred);
    }

    public async Task Cancel()
    {
        var userKey = GetUserCacheKey();

        if (_uploadTokens.TryGetValue(userKey, out var cts))
        {
            cts.Cancel();
            _uploadTokens.Remove(userKey);
        }

        if (_cache.TryGetValue(userKey, out UploadResult command))
        {
            var s3Obj = new S3Object
            {
                BucketName = command.BucketName,
                Name = command.ObjectName
            };

            var cred = new AwsCredentials
            {
                AwsAccessKey = _configuration.GetSection("AWS")["AccessKey"],
                AwsSecretKey = _configuration.GetSection("AWS")["SecretKey"],
                ServiceUrl = _configuration.GetSection("AWS")["ServiceURL"]
            };

            await _storageService.AbortMultipartUploadAsync(s3Obj, cred, command.UploadId);

            _cache.Remove(userKey);
        }

    }
    
}