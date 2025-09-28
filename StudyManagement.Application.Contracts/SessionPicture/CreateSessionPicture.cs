using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StudyManagement.Application.Contracts.SessionPicture
{
    public class CreateSessionPicture
    {
        public long SessionId { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxFileSize(5*1024*1024,ErrorMessage = ValidationMessages.MaxFileSizePicture)]
        public IFormFile Picture { get; set; }
    }
}
