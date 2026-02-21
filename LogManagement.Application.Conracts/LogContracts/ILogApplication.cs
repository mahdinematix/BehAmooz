namespace LogManagement.Application.Contracts.LogContracts
{
    public interface ILogApplication
    {
        void Create(CreateLog command);
        List<LogViewModel> Search(LogSearchModel searchMod, long currentAccountUniversityId, string currentAccountRole);

        List<LogViewModel> GetCourseLogsById(long id);
        List<LogViewModel> GetClassLogsById(long id);
        List<LogViewModel> GetSessionLogsById(long id);
    }
}
