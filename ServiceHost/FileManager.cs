using _01_Framework.Application;
using _01_Framework.Application.AwsServices;
using _01_Framework.Application.AwsServices.AwsDto;
using Microsoft.Extensions.Caching.Memory;
using S3Object = _01_Framework.Application.AwsServices.AwsDto.S3Object;

namespace ServiceHost;

public class FileManager : IFileManager
{
    private readonly IConfiguration _configuration;
    private readonly IStorageService _storageService;
    private readonly IAuthHelper _authHelper;
    static Dictionary<string, CancellationTokenSource> _uploadTokens = new();
    static MemoryCache _cache = new(new MemoryCacheOptions());


    public FileManager(IStorageService storageService, IConfiguration configuration, IAuthHelper authHelper)
    {
        _storageService = storageService;
        _configuration = configuration;
        _authHelper = authHelper;
    }

    private string GetUserCacheKey()
    {
        var userId = _authHelper.CurrentAccountId();
        return $"upload_user_{userId}";
    }


    public async Task<string> Upload(IFormFile file, bool isVideo)
    {
        var userKey = GetUserCacheKey();
        _cache.Remove($"{userKey}_canceled");

        if (_cache.TryGetValue($"{userKey}_canceled", out bool canceled) && canceled)
        {
            return "";
        }

        if (file == null) return "";


        var bucketName = _configuration.GetSection("AWS")["BucketName"];
        await using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        var fileName = Path.GetFileName(file.FileName);
        var objName = $"{Guid.NewGuid()}-{fileName}";
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



        var cts = new CancellationTokenSource();
        _uploadTokens.Add(userKey, cts);

        var uploadId = await _storageService.InitiateUploadAsync(s3Obj, cred);

        _cache.Set(userKey, new UploadResult
        {
            BucketName = bucketName,
            ObjectName = objName,
            UploadId = uploadId,
            UploadStatus = UploadStatus.InProgress
        }, TimeSpan.FromMinutes(30));

        try
        {
            await _storageService.UploadPartsAsync(s3Obj, cred, isVideo, cts.Token, uploadId);

            if (cts.Token.IsCancellationRequested)
            {
                return "";
            }

            if (_cache.TryGetValue(userKey, out UploadResult result))
            {
                result.UploadStatus = UploadStatus.Completed;
                _cache.Set(userKey, result, TimeSpan.FromMinutes(30));
            }

            var url = _storageService.GetObject(s3Obj, cred);
            return string.IsNullOrWhiteSpace(url) ? "" : url;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Upload canceled by user.");
            return "";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Upload error: {ex.Message}");
            return "";
        }
        finally
        {
            memoryStream?.Dispose();
            _uploadTokens.Remove(userKey);
        }
    }


    public async Task Cancel()
    {
        var userKey = GetUserCacheKey();


        if (_cache.TryGetValue($"{userKey}_canceled", out bool alreadyCanceled) && alreadyCanceled)
        {
            return;
        }

        _cache.Set($"{userKey}_canceled", true, TimeSpan.FromMinutes(10));


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
                Name = command.ObjectName,
                InputStream = null
            };

            var cred = new AwsCredentials
            {
                AwsAccessKey = _configuration.GetSection("AWS")["AccessKey"],
                AwsSecretKey = _configuration.GetSection("AWS")["SecretKey"],
                ServiceUrl = _configuration.GetSection("AWS")["ServiceURL"]
            };
            await _storageService.AbortUploadAsync(s3Obj, cred, command.UploadId);
            _cache.Remove(userKey);
        }

    }
}