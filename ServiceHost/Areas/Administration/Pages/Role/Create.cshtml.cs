using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Pages.Role
{
    public class CreateModel : UserContextPageModel
    {

        public CreateRole Command;
        private readonly IRoleApplication _roleApplication;
        
        public CreateModel(IRoleApplication roleApplication, IAuthHelper authHelper) : base(authHelper)
        {
            _roleApplication = roleApplication;
        }

        public IActionResult OnGet()
        {

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
        [NeedsPermissions(AccountPermissions.CreateRole)]
        public IActionResult OnPost(CreateRole command)
        {
            var result = _roleApplication.Create(command);
            return RedirectToPage("./Index");
        }
    }
}
