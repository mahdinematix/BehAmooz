using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Course
{
    public class CreateCourse
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Name { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int NumberOfUnit { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string CourseKind { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(10,ErrorMessage = ValidationMessages.MaxLength)]
        public string Code { get; set; }

    }
}