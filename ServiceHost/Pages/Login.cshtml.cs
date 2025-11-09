using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;
        public Login Command;

        public LoginModel(IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }

        [TempData] public string LoginMessage { get; set; }
        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();
            if (_authHelper.IsAuthenticated() && _authHelper.CurrentAccountRole() == Roles.Student)
            {
                return RedirectToPage("/Index");
            }
            if (_authHelper.IsAuthenticated() && _authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            return Page();
        }

        public IActionResult OnPost(Login command)
        {
            var result = _accountApplication.Login(command);
            if (result.IsSucceeded)
            {
                if (_authHelper.CurrentAccountRole() != Roles.Student)
                {
                    return RedirectToPage("/Index", new { area = "Administration" });
                }
                return RedirectToPage("/Index");
            }
            LoginMessage = result.Message;
            return RedirectToPage("/Login");
        }

        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("/Login");
        }
    }
}
