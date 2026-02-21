using _01_Framework.Domain;
using AccountManagement.Application.Contract.Role;

namespace AccountManagement.Domain.RoleAgg
{
    public interface IRoleRepository : IRepositoryBase<long,Role>
    {
        List<RoleViewModel> GetAllRoles(string currentAccountRole);
        EditRole GetDetails(long id);
    }
}
