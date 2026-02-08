using LogManagement.Application.Contracts.Log;
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

        public List<LogViewModel> Search(LogSearchModel searchModel)
        {
            return _logRepository.Search(searchModel);
        }

        public List<LogViewModel> GetCourseLogsById(long id)
        {
            return _logRepository.GetCourseLogsById(id);
        }

        public List<LogViewModel> GetClassLogsById(long id)
        {
            return _logRepository.GetClassLogsById(id);
        }

        public List<LogViewModel> GetSessionLogsById(long id)
        {
            return _logRepository.GetSessionLogsById(id);
        }


    }
}
