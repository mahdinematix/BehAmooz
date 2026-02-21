using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;
using _01_Framework.Infrastructure;
using StudyManagement.Application.Contracts.University;

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
        public int UniversityTypeId { get; set; }
        public long UniversityId { get; set; }
        public bool ForAllUniversities { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<UniversityTypeViewModel> UniversityTypes
        { get; set; }
        public List<UniversityViewModel> Universities { get; set; }
    }
}
