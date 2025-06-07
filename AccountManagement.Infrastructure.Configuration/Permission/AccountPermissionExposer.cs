using _01_Framework.Infrastructure;

namespace AccountManagement.Infrastructure.Configuration.Permission
{
    public class AccountPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Account", new List<PermissionDto>
                    {
                        new PermissionDto(AccountPermissions.ListAccounts,"ListAccounts"),
                        new PermissionDto(AccountPermissions.CreateAccount,"CreateAccount"),
                        new PermissionDto(AccountPermissions.EditAccount,"EditAccount"),
                        new PermissionDto(AccountPermissions.ChangePassword,"ChangePassword"),
                        new PermissionDto(AccountPermissions.ConfirmOrReject,"ConfirmOrReject"),

                    }
                },
                {
                    "Role", new List<PermissionDto>
                    {
                        new PermissionDto(AccountPermissions.ListRoles,"ListRoles"),
                        new PermissionDto(AccountPermissions.CreateRole,"CreateRole"),
                        new PermissionDto(AccountPermissions.EditRole,"EditRole")
                    }
                }
            };
        }
    }
}
