using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace MessageManagement.Application.Contract.Message
{
    public class CreateMessage
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(100, ErrorMessage = ValidationMessages.MaxLength)]
        public string Title { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(5000, ErrorMessage = ValidationMessages.MaxLength)]
        public string Body { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(20, ErrorMessage = ValidationMessages.MaxLength)]
        public string MessageFor { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
