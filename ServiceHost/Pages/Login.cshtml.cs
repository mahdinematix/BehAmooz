using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class LoginModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        public FirstLogin Command;


        public LoginModel(IAccountApplication accountApplication, IAuthHelper authHelper, IHttpClientFactory httpClientFactory):base(authHelper)
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

        public async Task<IActionResult> OnPost(FirstLogin command)
        {
            var result = _accountApplication.FirstLogin(command);

            if (result.IsSucceeded)
            {
                return RedirectToPage("/SMS", new{ nationalCode = command.NationalCode, type = OtpType.Login});
            }
            LoginMessage = result.Message;

            return RedirectToPage("/Login");
        }

       

        public async Task<IActionResult> OnGetLogout()
        {
            await _accountApplication.Logout();
            TempData.Remove("AdminFirstRedirect");
            return RedirectToPage("/Login");
        }
    }
}
