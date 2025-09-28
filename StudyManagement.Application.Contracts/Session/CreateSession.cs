using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StudyManagement.Application.Contracts.Session
{
    public class CreateSession
    {
        [Range(1,16,ErrorMessage = ValidationMessages.MaxLength)]
        public int Number { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(200, ErrorMessage = ValidationMessages.MaxLength)]
        public string Title { get; set; }
        [MaxFileSize(1024*1024*1024,ErrorMessage = ValidationMessages.MaxFileSizeVideo)]
        public IFormFile? Video { get; set; }
        [MaxFileSize(100*1024*1024, ErrorMessage = ValidationMessages.MaxFileSizeBooklet)]
        public IFormFile? Booklet { get; set; }
        [MaxLength(5000, ErrorMessage = ValidationMessages.MaxLength)]
        public string? Description { get; set; }
        public long ClassId { get; set; }
    }
}
