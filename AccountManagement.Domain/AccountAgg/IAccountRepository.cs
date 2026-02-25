using _01_Framework.Domain;
using AccountManagement.Application.Contract.Account;

namespace AccountManagement.Domain.AccountAgg
{
    public interface IAccountRepository : IRepositoryBase<long,Account>
    {
        EditAccount GetDetails(long id);
        List<AccountViewModel> Search(AccountSearchModel searchModel, string currentAccountRole, long currentAccountUniversityId, int currentAccountTypeUniversity);
        Account GetByNationalCode(string nationalCode);
        List<AccountViewModel> GetProfessors(string currentAccountRole, long currentAccountUniversityId);
        string GetProfessorById(long professorId);
        List<AccountViewModel> SearchInStudents(AccountSearchModel searchModel);
        List<AccountViewModel> SearchInCustomers(AccountSearchModel searchModel);
    }
}
