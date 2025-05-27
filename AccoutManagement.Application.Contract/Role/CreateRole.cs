using _01_Framework.Application;
using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Application.Contract.Role
{
    public class CreateRole
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(50, ErrorMessage = ValidationMessages.MaxLength)]
        public string Name { get; set; }
        public List<int> Permissions { get; set; }

    }
}
