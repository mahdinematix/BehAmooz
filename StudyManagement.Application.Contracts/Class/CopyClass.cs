using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace StudyManagement.Application.Contracts.Class
{
    public class CopyClass
    {
        public long ClassId { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(20, ErrorMessage = ValidationMessages.MaxLength)]
        public string ClassCode { get; set; }
        public List<ClassViewModel> Classes { get; set; }
    }
}
