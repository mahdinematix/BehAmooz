using _01_Framework.Infrastructure;

namespace MessageManagement.Infrastructure.Configuration.Permission
{
    public class MessagePermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Message", new List<PermissionDto>
                    {
                        new PermissionDto(MessagePermissions.ListMessages,"ListMessages"),
                        new PermissionDto(MessagePermissions.CreateMessage,"CreateMessage"),
                        new PermissionDto(MessagePermissions.EditMessage,"EditMessage")
                    }
                }
            };
        }
    }
}
