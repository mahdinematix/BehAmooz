using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Pages.Role
{
    public class IndexModel : UserContextPageModel
    {
        public List<RoleViewModel> RolesList;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IRoleApplication roleApplication, IAuthHelper authHelper):base(authHelper)
        {
            _roleApplication = roleApplication;
        }

        [NeedsPermissions(AccountPermissions.ListRoles)]
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

            if (CurrentAccountRole!=Roles.SuperAdministrator)
            {
                return RedirectToPage("/Account/Index", new {area = "Administration"});
            }

            RolesList = _roleApplication.GetAllRoles(CurrentAccountRole);
            return Page();
        }
    }
}
