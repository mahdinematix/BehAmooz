using _01_Framework.Application;
using _01_Framework.Application.AwsServices;
using _01_Framework.Application.AwsServices.AwsDto;
using _01_Framework.Application.TusServices;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using S3Object = _01_Framework.Application.AwsServices.AwsDto.S3Object;

namespace ServiceHost;

public class FileManager : IFileManager
{
    private readonly IConfiguration _configuration;
    private readonly IStorageServiceAws _storageServiceAws;
    private readonly IStorageServiceTus _storageServiceTus;
    private readonly IAuthHelper _authHelper;
    static Dictionary<string, CancellationTokenSource> _uploadTokens = new();
    static Dictionary<string, CancellationTokenSource> _tusUploadTokens = new();

    static MemoryCache _cache = new(new MemoryCacheOptions());

    public FileManager(IStorageServiceAws storageServiceAws, IConfiguration configuration, IAuthHelper authHelper, IStorageServiceTus storageServiceTus)
    {
        _storageServiceAws = storageServiceAws;
        _configuration = configuration;
        _authHelper = authHelper;
        _storageServiceTus = storageServiceTus;
    }

    private string GetUserCacheKey()
    {
        var userId = _authHelper.CurrentAccountId();
        return $"upload_user_{userId}";
    }


    public async Task<string> Upload(IFormFile file, bool isVideo)
    {
        if (!isVideo)
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
            var objName = $"{Guid.NewGuid()}-{DateTime.Now}-{fileName}";
            var s3Obj = new S3Object
            {
                InputStream = memoryStream,
                BucketName = bucketName,
                Name = objName,
                ContentType = file.ContentType
            };
            var cred = new AwsCredentials
            {
                AwsAccessKey = _configuration.GetSection("AWS")["AccessKey"],
                AwsSecretKey = _configuration.GetSection("AWS")["SecretKey"],
                ServiceUrl = _configuration.GetSection("AWS")["ServiceURL"]
            };

            var cts = new CancellationTokenSource();
            _uploadTokens.Add(userKey, cts);

            var uploadId = await _storageServiceAws.InitiateUploadAsync(s3Obj, cred);

            _cache.Set(userKey, new UploadResult
            {
                BucketName = bucketName,
                ObjectName = objName,
                UploadId = uploadId,
                UploadStatus = UploadStatus.InProgress
            }, TimeSpan.FromMinutes(30));

            try
            {
                await _storageServiceAws.UploadPartsAsync(s3Obj, cred, cts.Token, uploadId);

                if (cts.Token.IsCancellationRequested)
                {
                    return "";
                }

                if (_cache.TryGetValue(userKey, out UploadResult result))
                {
                    result.UploadStatus = UploadStatus.Completed;
                    _cache.Set(userKey, result, TimeSpan.FromMinutes(30));
                }

                var url = _storageServiceAws.GetObject(s3Obj, cred);
                return string.IsNullOrWhiteSpace(url.Result) ? "" : url.Result;
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
        else
        {
            var userKey = GetUserCacheKey();
            _cache.Remove($"{userKey}_canceled");

            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            var fileName = Path.GetFileName(file.FileName);
            var objName = $"{Guid.NewGuid()}-{DateTime.Now}-{fileName}";
            var obj = new S3Object
            {
                Name = objName,
                InputStream = stream,
                ContentType = file.ContentType
            };

            var metadata = BuildTusMetadata(obj.Name, obj.ContentType);

            string uploadUrl = await _storageServiceTus.InitiateUploadAsync(obj, metadata);
            string fileId = uploadUrl.Split('/').Last();

            _cache.Set($"{userKey}_arvan_fileId", fileId, TimeSpan.FromMinutes(30));
            var tusCts = new CancellationTokenSource();
            _tusUploadTokens[userKey] = tusCts;

            try
            {
                await _storageServiceTus.UploadPartsAsync(obj, uploadUrl, tusCts.Token);
            }
            catch (OperationCanceledException)
            {
                
                return "";
            }



            if (_cache.TryGetValue($"{userKey}_canceled", out bool canceled) && canceled)
            {
                await _storageServiceTus.DeleteFileAsync(fileId);
                return "";
            }


            string videoId = await _storageServiceTus.FinalizeUploadAsync(obj.Name, fileId);

            _tusUploadTokens.Remove(userKey);


            _cache.Set($"{userKey}_arvan_videoId", videoId, TimeSpan.FromMinutes(30));

            var url = await _storageServiceTus.GetPlayerUrlAsync(videoId);
            return url;
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

        if (_cache.TryGetValue($"{userKey}_arvan_fileId", out string fileId))
        {
            await _storageServiceTus.DeleteFileAsync(fileId);
            _cache.Remove($"{userKey}_arvan_fileId");
        }

        if (_cache.TryGetValue($"{userKey}_arvan_videoId", out string videoId))
        {
            await _storageServiceTus.DeleteVideoAsync(videoId);
            _cache.Remove($"{userKey}_arvan_videoId");
        }


        if (_uploadTokens.TryGetValue(userKey, out var cts))
        {
            cts.Cancel();
            _uploadTokens.Remove(userKey);
        }

        if (_tusUploadTokens.TryGetValue(userKey, out var tusCts))
        {
            tusCts.Cancel();
            _tusUploadTokens.Remove(userKey);
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
            await _storageServiceAws.AbortUploadAsync(s3Obj, cred, command.UploadId);
            _cache.Remove(userKey);
        }


    }
    private string BuildTusMetadata(string fileName, string mimeType)
    {
        string fileNameBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileName));
        string mimeBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(mimeType));

        return $"filename {fileNameBase64},filetype {mimeBase64}";
    }
}