using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Pages
{
    public class ChangePasswordModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        public ChangePassword Command;
        [TempData] public string Message { get; set; } 

        public ChangePasswordModel(IAuthHelper authHelper, IAccountApplication accountApplication) : base(authHelper)
        {
            _accountApplication = accountApplication;
        }

        public IActionResult OnGet()
        {
            if (IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Index", new {area = "Administration"});
            }

            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            return Page();
        }

        public IActionResult OnPost(ChangePassword command)
        {
            command.Id = CurrentAccountId;
            var result = _accountApplication.ChangePasswordByUser(command);
            if (result.IsSucceeded)
            {
                Message = result.Message;
                return RedirectToPage("/ChangePassword");
            }

            Message = result.Message;
            return RedirectToPage("/ChangePassword");

        }
    }
}
