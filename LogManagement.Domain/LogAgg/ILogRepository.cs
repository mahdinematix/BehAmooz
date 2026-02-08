using _01_Framework.Domain;
using LogManagement.Application.Contracts.Log;

namespace LogManagement.Domain.LogAgg
{
    public interface ILogRepository : IRepositoryBase<long,Log>
    {
        List<LogViewModel> Search(LogSearchModel searchModel);
        List<LogViewModel> GetCourseLogsById(long id);
        List<LogViewModel> GetClassLogsById(long id);
        List<LogViewModel> GetSessionLogsById(long id);

    }
}
