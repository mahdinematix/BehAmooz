using _01_Framework.Application.AwsServices.AwsDto;

namespace _01_Framework.Application.TusServices;

public interface IStorageServiceTus
{
    Task<string> InitiateUploadAsync(S3Object obj, string metadata);
    Task UploadPartsAsync(S3Object obj, string uploadUrl, CancellationToken ct);
    Task<string> FinalizeUploadAsync(string fileName, string fileId);
    Task<string> GetPlayerUrlAsync(string videoId);
    Task DeleteFileAsync(string fileId);
    Task DeleteVideoAsync(string videoId);
}

