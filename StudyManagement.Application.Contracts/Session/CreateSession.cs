using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace StudyManagement.Application.Contracts.Session
{
    public class CreateSession
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(2, ErrorMessage = ValidationMessages.MaxLength)]
        public string Number { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(200, ErrorMessage = ValidationMessages.MaxLength)]
        public string Title { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLength)]
        public string Video { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLength)]
        public string Booklet { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(1000, ErrorMessage = ValidationMessages.MaxLength)]
        public string Description { get; set; }
        public long ClassId { get; set; }
    }
}
