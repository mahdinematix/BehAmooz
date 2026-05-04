using _01_Framework.Application;

namespace AccountManagement.Application.Contract.Account
{
    public interface IAccountApplication
    {
        Task<OperationResult> Register(RegisterAccount command);
        Task<OperationResult> Edit(EditAccount command);
        OperationResult Confirm(long id);
        OperationResult Reject(long id);
        OperationResult ChangePassword(ChangePassword command);
        OperationResult ChangePasswordByUser(ChangePassword command);
        OperationResult ResetPassword(ResetPassword command);
        EditAccount GetDetails(long id);
        List<AccountViewModel> Search(AccountSearchModel searchModel, string currentAccountRole, long currentAccountUniversityId, int currentAccountTypeUniversity);
        OperationResult FirstLogin(FirstLogin command);
        Task FinalLogin(string nationalCode);
        Task Logout();
        List<AccountViewModel> GetProfessors(string currentAccountRole, long currentAccountUniversityId);
        string GetProfessorById(long professorId);
        List<AccountViewModel> SearchInStudents(AccountSearchModel searchModel);
        List<AccountViewModel> SearchInCustomers(AccountSearchModel searchModel);
        string GetPhoneNumberByNationalCode(string nationalCode);
        OperationResult ExistsNationalCode(string nationalCode);
    }
}
