using System.Collections;
using _01_Framework.Application;

namespace AccountManagement.Application.Contract.Role
{
    public interface IRoleApplication
    {
        OperationResult Create(CreateRole command);
        OperationResult Edit(EditRole  command);
        List<RoleViewModel> GetAllRoles(string currentAccountRole);
        EditRole GetDetails(long id);
    }
}
