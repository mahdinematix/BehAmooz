using _01_Framework.Infrastructure;

namespace StudyManagement.Infrastructure.Configuration.Permissions
{
    public class StudyPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDto>> Expose()
        {
            return new Dictionary<string, List<PermissionDto>>
            {
                {
                    "University", new List<PermissionDto>
                    {
                        new PermissionDto(StudyPermissions.ListUniversity,"ListUniversity"),
                        new PermissionDto(StudyPermissions.CreateUniversity,"CreateUniversity"),
                        new PermissionDto(StudyPermissions.EditUniversity,"EditUniversity"),
                        new PermissionDto(StudyPermissions.SetCurrentSemester,"SetCurrentSemester"),
                        new PermissionDto(StudyPermissions.ActivateUniversity,"ActivateUniversity"),
                        new PermissionDto(StudyPermissions.DeactivateUniversity,"DeactivateUniversity"),
                    }
                }
            };
        }
    }
}
