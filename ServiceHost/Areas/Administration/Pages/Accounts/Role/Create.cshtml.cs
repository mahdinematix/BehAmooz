using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class CreateModel : PageModel
    {

        public CreateRole Command;
        private readonly IRoleApplication _roleApplication;
        private readonly IAuthHelper _authHelper;
        
        public CreateModel(IRoleApplication roleApplication, IAuthHelper authHelper)
        {
            _roleApplication = roleApplication;
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();

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
        [NeedsPermissions(AccountPermissions.CreateRole)]
        public IActionResult OnPost(CreateRole command)
        {
            var result = _roleApplication.Create(command);
            return RedirectToPage("./Index");
        }
    }
}
