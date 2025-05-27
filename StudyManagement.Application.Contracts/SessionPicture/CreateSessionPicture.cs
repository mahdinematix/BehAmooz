using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace StudyManagement.Application.Contracts.SessionPicture
{
    public class CreateSessionPicture
    {
        public long SessionId { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLength)]
        public string Picture { get; set; }
    }
}
