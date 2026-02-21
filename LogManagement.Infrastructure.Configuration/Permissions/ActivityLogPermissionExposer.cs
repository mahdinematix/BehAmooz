using _01_Framework.Infrastructure;

namespace LogManagement.Infrastructure.Configuration.Permissions
{
    public class ActivityLogPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "ActivityLog", new List<PermissionDto>
                    {
                        new PermissionDto(ActivityLogPermissions.ShowActivityLog,"ShowActivityLogs")
                    }
                }
            };
        }
    }
}
