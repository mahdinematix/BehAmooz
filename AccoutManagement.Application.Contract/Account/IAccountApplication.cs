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
    }
}
