using System.ComponentModel.DataAnnotations;
using _01_Framework.Application;

namespace AccountManagement.Application.Contract.Account;

public class Login
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [MaxLength(10,ErrorMessage = ValidationMessages.MaxLength)]
    [MinLength(10,ErrorMessage = ValidationMessages.MinLength)]
    public string NationalCode { get; set; }
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    public string Password { get; set; }
}