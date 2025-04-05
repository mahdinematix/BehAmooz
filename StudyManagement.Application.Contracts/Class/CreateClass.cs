using System.ComponentModel.DataAnnotations;
using _01_Framework.Application;
using StudyManagement.Application.Contracts.Course;

namespace StudyManagement.Application.Contracts.Class
{
    public class CreateClass
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(6,ErrorMessage = ValidationMessages.MaxLength)]
        public string Code { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string StartTime { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string EndTime { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public long CourseId { get; set; }
        public List<CourseViewModel> Courses { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Day { get; set; }
    }
}
