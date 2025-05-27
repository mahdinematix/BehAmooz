using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Course
{
    public class CreateCourse
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(300, ErrorMessage = ValidationMessages.MaxLength)]
        public string Name { get; set; }
        public int NumberOfUnit { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string CourseKind { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(20,ErrorMessage = ValidationMessages.MaxLength)]
        public string Code { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Major { get; set; }

    }
}