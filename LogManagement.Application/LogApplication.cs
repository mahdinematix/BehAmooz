using LogManagement.Application.Contracts.LogContracts;
using LogManagement.Domain.LogAgg;

namespace LogManagement.Application
{
    public class LogApplication : ILogApplication
    {
        private readonly ILogRepository _logRepository;

        public LogApplication(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void Create(CreateLog command)
        {
            var log = new Log(command.AccountId, command.Operation, command.TargetType,
                command.TargetId, command.Description);
            _logRepository.Create(log);
            _logRepository.Save();
        }

        public List<LogViewModel> Search(LogSearchModel searchModel, long currentAccountUniversityId,string currentAccountRole)
        {
            return _logRepository.Search(searchModel, currentAccountUniversityId, currentAccountRole);
        }

        public List<LogViewModel> GetCourseLogsById(long id)
        {
            return _logRepository.GetCourseLogsById(id);
        }

        public List<LogViewModel> GetClassLogsById(long id, long courseId)
        {
            return _logRepository.GetClassLogsById(id, courseId);
        }

        public List<LogViewModel> GetSessionLogsById(long id, long classId)
        {
            return _logRepository.GetSessionLogsById(id, classId);
        }

        public List<LogViewModel> GetUniversityLogsById(long id)
        {
            return _logRepository.GetUniversityLogsById(id);
        }
    }
}
