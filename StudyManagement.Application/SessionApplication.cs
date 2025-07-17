using _01_Framework.Application;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Application
{
    public class SessionApplication : ISessionApplication
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionApplication(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public OperationResult Create(CreateSession command)
        {
            var operation = new OperationResult();
            if (_sessionRepository.Exists(x=>x.Number == command.Number && x.ClassId == command.ClassId))
            {
                return operation.Failed(ApplicationMessages.ASessionWithThatNumberExists);
            }

            var session = new Session(command.Number, command.Title, command.Video, command.Booklet,
                command.Description,command.Price, command.ClassId);
            _sessionRepository.Create(session);
            _sessionRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditSession command)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(command.Id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_sessionRepository.Exists(x => x.Number == command.Number && x.Id != command.Id  && x.ClassId == command.ClassId))
            {
                return operation.Failed(ApplicationMessages.ASessionWithThatNumberExists);
            }

            session.Edit(command.Number, command.Title, command.Video, command.Booklet,
                command.Description,command.Price, command.ClassId);
            _sessionRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Activate(long id)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            session.Activate();
            _sessionRepository.Save();
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            session.DeActivate();
            _sessionRepository.Save();
            return operation.Succeed();
        }

        public EditSession GetDetails(long id)
        {
            return _sessionRepository.GetDetails(id);
        }

        public List<SessionViewModel> GetAllByClassId(long classId)
        {
            return _sessionRepository.GetAllByClassId(classId);
        }

        public SessionViewModel GetBySessionId(long sessionId)
        {
            return _sessionRepository.GetBySessionId(sessionId);
        }
    }
}
