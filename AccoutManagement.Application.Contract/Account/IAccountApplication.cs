using System.Collections;
using _01_Framework.Application;

namespace AccountManagement.Application.Contract.Account
{
    public interface IAccountApplication
    {
        OperationResult Register(RegisterAccount command);
        OperationResult Edit(EditAccount command);
        OperationResult Confirm(long id);
        OperationResult Reject(long id);
        OperationResult ChangePassword(ChangePassword command);
        EditAccount GetDetails(long id);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
        OperationResult Login(Login command);
        void Logout();
        List<AccountViewModel> GetProfessors();
        string GetProfessorById(long professorId);
        List<AccountViewModel> SearchInStudents(AccountSearchModel searchModel);
    }
}
