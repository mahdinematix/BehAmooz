using Microsoft.AspNetCore.Http;

namespace _01_Framework.Application
{
    public interface IFileManager
    {
        Task<string> Upload(IFormFile file, bool isVideo);
        Task Cancel();
    }
}
