namespace LogManagement.Application.Contracts.LogContracts
{
    public interface ILogApplication
    {
        void Create(CreateLog command);
        List<LogViewModel> Search(LogSearchModel searchMod, long currentAccountUniversityId, string currentAccountRole);

        List<LogViewModel> GetCourseLogsById(long id);
        List<LogViewModel> GetClassLogsById(long id, long courseId);
        List<LogViewModel> GetSessionLogsById(long id, long classId);
        List<LogViewModel> GetUniversityLogsById(long id);
    }
}
