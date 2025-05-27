using _01_Framework.Application;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRoleRepository _roleRepository;

        public RoleApplication(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public OperationResult Create(CreateRole command)
        {
            var operation = new OperationResult();
            if (_roleRepository.Exists(x=>x.Name == command.Name))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var role = new Role(command.Name, new List<RolePermission>());
            _roleRepository.Create(role);
            _roleRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditRole command)
        {
            var operation = new OperationResult();
            var role = _roleRepository.GetBy(command.Id);

            if (role == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            }

            if (_roleRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var permissions = new List<RolePermission>();
            command.Permissions.ForEach(code => permissions.Add(new RolePermission(code)));

            role.Edit(command.Name, permissions);
            _roleRepository.Save();
            return operation.Succeed();
        }

        public List<RoleViewModel> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public EditRole GetDetails(long id)
        {
            return _roleRepository.GetDetails(id);
        }
    }
}
