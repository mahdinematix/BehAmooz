using _01_Framework.Domain;
using AccountManagement.Application.Contract.Account;

namespace AccountManagement.Domain.AccountAgg
{
    public interface IAccountRepository : IRepositoryBase<long,Account>
    {
        EditAccount GetDetails(long id);
        List<AccountViewModel> Search(AccountSearchModel searchModel);
    }
}
