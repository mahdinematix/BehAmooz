using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class IndexModel : PageModel
    {
        public List<RoleViewModel> Roles;
        private readonly IRoleApplication _roleApplication;
        private readonly IAuthHelper _authHelper;

        public IndexModel(IRoleApplication roleApplication, IAuthHelper authHelper)
        {
            _roleApplication = roleApplication;
            _authHelper = authHelper;
        }

        [NeedsPermissions(AccountPermissions.ListRoles)]
        public IActionResult OnGet()
        {
            if (_authHelper.CurrentAccountRole() != _01_Framework.Infrastructure.Roles.Administrator)
            {
                return RedirectToPage("/Index");
            }
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Roles = _roleApplication.GetAllRoles();
            return Page();
        }
    }
}
