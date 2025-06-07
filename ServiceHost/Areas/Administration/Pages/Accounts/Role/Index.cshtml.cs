using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Role
{
    public class IndexModel : PageModel
    {
        public List<RoleViewModel> Roles;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IRoleApplication roleApplication)
        {
            
            _roleApplication = roleApplication;
        }

        [NeedsPermissions(AccountPermissions.ListRoles)]
        public void OnGet()
        {
            Roles = _roleApplication.GetAllRoles();
        }
    }
}
