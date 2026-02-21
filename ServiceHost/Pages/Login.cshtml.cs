using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class LoginModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        public Login Command;

        public LoginModel(IAccountApplication accountApplication, IAuthHelper authHelper):base(authHelper)
        {
            _accountApplication = accountApplication;
        }

        [TempData] public string LoginMessage { get; set; }
        public IActionResult OnGet()
        {
            if (IsAuthenticated)
            {
                if (CurrentAccountRole == Roles.Student)
                {
                    return RedirectToPage("/Index");
                }
                if (CurrentAccountRole == Roles.Administrator || CurrentAccountRole == Roles.SuperAdministrator)
                {
                    return RedirectToPage("/Index", new { area = "Administration" });
                }
                if (CurrentAccountRole == Roles.Professor)
                {
                    return RedirectToPage("/Index", new { area = "Professor" });
                }

                if (CurrentAccountStatus == Statuses.Waiting)
                {
                    return RedirectToPage("/NotConfirmed");
                }

                if (CurrentAccountStatus == Statuses.Rejected)
                {
                    return RedirectToPage("/Reject");
                }
            }

            return Page();
        }

        public IActionResult OnPost(Login command)
        {
            var result = _accountApplication.Login(command);

            if (result.IsSucceeded)
            {
                return RedirectToPage("/Index");
            }
            LoginMessage = result.Message;
            return RedirectToPage("/Login");
        }

        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            TempData.Remove("AdminFirstRedirect");
            return RedirectToPage("/Login");
        }
    }
}
