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
        [Range(1,int.MaxValue,ErrorMessage = ValidationMessages.IsRequired)]
        public int Major { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int UniversityType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int University { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int Price { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int EducationLevel { get; set; }
    }
}