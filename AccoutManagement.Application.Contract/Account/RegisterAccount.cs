using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using _01_Framework.Application;
using AccountManagement.Application.Contract.Role;

namespace AccountManagement.Application.Contract.Account
{
    public class RegisterAccount
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(150, ErrorMessage = ValidationMessages.MaxLength)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(200, ErrorMessage = ValidationMessages.MaxLength)]
        public string LastName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(300, ErrorMessage = ValidationMessages.MaxLength)]
        public string Email { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(14, ErrorMessage = ValidationMessages.MaxLength)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(10, ErrorMessage = ValidationMessages.MaxLength)]
        public string NationalCode { get; set; }
        public int UniversityType { get; set; }
        public int University { get; set; }
        public int Major { get; set; }
        public string NationalCardPicture { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(20, ErrorMessage = ValidationMessages.MaxLength)]
        public string Code { get; set; }
        public long RoleId { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
