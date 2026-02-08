using _01_Framework.Infrastructure;

namespace LogManagement.Infrastructure.Configuration.Permissions
{
    public class LogPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "Log", new List<PermissionDto>
                    {
                        new PermissionDto(LogPermissions.ShowLogs,"ShowLogs")
                    }
                }
            };
        }
    }
}
