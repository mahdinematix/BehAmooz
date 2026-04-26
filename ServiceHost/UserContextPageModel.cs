using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost
{
    public abstract class UserContextPageModel:PageModel
    {
        protected readonly IAuthHelper AuthHelper;

        protected UserContextPageModel(IAuthHelper authHelper)
        {
            AuthHelper = authHelper;
        }

        protected long CurrentAccountId => AuthHelper.CurrentAccountId();
        protected long CurrentAccountUniversityId => AuthHelper.CurrentAccountUniversityId();
        protected string CurrentAccountRole => AuthHelper.CurrentAccountRole();
        protected string CurrentAccountNationalCode => AuthHelper.GetAccountInfo().NationalCode;
        protected int CurrentAccountStatus => AuthHelper.CurrentAccountStatus();
        protected AuthViewModel CurrentAccountInfo => AuthHelper.GetAccountInfo();

        protected bool IsAuthenticated => AuthHelper.IsAuthenticated();

        protected bool IsSuperAdmin => CurrentAccountRole == Roles.SuperAdministrator;
        protected bool IsAdmin => CurrentAccountRole == Roles.Administrator;
        protected bool IsProfessor => CurrentAccountRole == Roles.Professor;
        protected bool IsStudent => CurrentAccountRole == Roles.Student;
    }
}
