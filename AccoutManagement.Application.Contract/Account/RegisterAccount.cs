using System.ComponentModel.DataAnnotations;
using _01_Framework.Application;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Http;

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
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string Password { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
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
        [Range(1,int.MaxValue,ErrorMessage = ValidationMessages.IsRequired)]
        public int UniversityType { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int University { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public int Major { get; set; }
        [MaxFileSize(5 * 1024 * 1024 , ErrorMessage = ValidationMessages.MaxFileSizePicture)]
        public IFormFile NationalCardPicture { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(20, ErrorMessage = ValidationMessages.MaxLength)]
        public string Code { get; set; }
        [Range(1, long.MaxValue, ErrorMessage = ValidationMessages.IsRequired)]
        public long RoleId { get; set; }
        public List<RoleViewModel> Roles { get; set; }
    }
}
