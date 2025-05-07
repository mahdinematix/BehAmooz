using _01_Framework.Application;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;
using StudyManagement.Domain.SessionPictureAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudyManagement.Application
{
    public class SessionPictureApplication : ISessionPictureApplication
    {
        private readonly ISessionPictureRepository _sessionPictureRepository;

        public SessionPictureApplication(ISessionPictureRepository sessionPictureRepository)
        {
            _sessionPictureRepository = sessionPictureRepository;
        }

        public OperationResult Create(CreateSessionPicture command)
        {
            var operation = new OperationResult();
            var sessionPicture = new SessionPicture(command.SessionId, command.Picture);
            _sessionPictureRepository.Create(sessionPicture);
            _sessionPictureRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditSessionPicture command)
        {
            var operation = new OperationResult();
            var sessionPicture = _sessionPictureRepository.GetBy(command.Id);
            if (sessionPicture == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            sessionPicture.Edit(command.SessionId,command.Picture);
            _sessionPictureRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Remove(long id)
        {
            var operation = new OperationResult();
            var sessionPicture = _sessionPictureRepository.GetBy(id);
            if (sessionPicture == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            sessionPicture.Remove();
            _sessionPictureRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Restore(long id)
        {
            var operation = new OperationResult();
            var sessionPicture = _sessionPictureRepository.GetBy(id);
            if (sessionPicture == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            sessionPicture.Restore();
            _sessionPictureRepository.Save();
            return operation.Succeed();
        }

        public EditSessionPicture GetDetails(long id)
        {
            return _sessionPictureRepository.GetDetails(id);
        }

        public List<SessionPictureViewModel> GetSessionPicturesBySessionId(long sessionId)
        {
            return _sessionPictureRepository.GetSessionPicturesBySessionId(sessionId);
        }
    }
}
