using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly IAccountApplication _accountApplication;
        public ChangePassword Command;
        [TempData] public string Message { get; set; } 

        public ChangePasswordModel(IAuthHelper authHelper, IAccountApplication accountApplication)
        {
            _authHelper = authHelper;
            _accountApplication = accountApplication;
        }

        public IActionResult OnGet()
        {

            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Index", new {area = "Administration"});
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

        public IActionResult OnPost(ChangePassword command)
        {
            command.Id = _authHelper.CurrentAccountId();
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
