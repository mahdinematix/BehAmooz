using _01_Framework.Domain;
using LogManagement.Application.Contracts.LogContracts;

namespace LogManagement.Domain.LogAgg
{
    public interface ILogRepository : IRepositoryBase<long,Log>
    {
        List<LogViewModel> Search(LogSearchModel searchModel, long currentAccountUniversityId, string currentAccountRole);
        List<LogViewModel> GetCourseLogsById(long id);
        List<LogViewModel> GetClassLogsById(long id, long courseId);
        List<LogViewModel> GetSessionLogsById(long id, long classId);
        List<LogViewModel> GetUniversityLogsById(long id);

    }
}
