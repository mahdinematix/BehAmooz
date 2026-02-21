using _01_Framework.Domain;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Domain.RoleAgg
{
    public class Role : EntityBase
    {
        public string Name { get; private set; }
        public List<Account> Accounts { get; private set; }
        public List<RolePermission> Permissions { get; private set; }

        protected Role()
        {
        }
        public Role(string name, List<RolePermission> permissions)
        {
            Name = name;
            Accounts = new List<Account>();
            Permissions = permissions;
        }

        public void Edit(List<RolePermission> permissions)
        {
            Permissions = permissions;
        }
    }
}
