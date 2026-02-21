
namespace _01_Framework.Application
{
    public interface IAuthHelper
    {
        void Signin(AuthViewModel account);
        void SignOut();
        bool IsAuthenticated();
        string CurrentAccountRole();
        AuthViewModel GetAccountInfo();
        List<int> GetPermissions();
        long CurrentAccountId(); 
        int CurrentAccountStatus();
        int CurrentAccountEducationLevel();
        long CurrentAccountUniversityId();
    }
}
