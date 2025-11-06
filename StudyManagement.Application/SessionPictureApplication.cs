using _01_Framework.Application;
using StudyManagement.Application.Contracts.SessionPicture;
using StudyManagement.Domain.SessionPictureAgg;

namespace StudyManagement.Application
{
    public class SessionPictureApplication : ISessionPictureApplication
    {
        private readonly ISessionPictureRepository _sessionPictureRepository;
        private readonly IFileManager _fileManager;

        public SessionPictureApplication(ISessionPictureRepository sessionPictureRepository, IFileManager fileManager)
        {
            _sessionPictureRepository = sessionPictureRepository;
            _fileManager = fileManager;
        }

        public async Task<OperationResult> CreateAsync(CreateSessionPicture command)
        {
            var operation = new OperationResult();
            var fileUrl = await _fileManager.Upload(command.Picture, false);
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                return operation.Failed(ApplicationMessages.UploadProgressCanceled);
            }
            var sessionPicture = new SessionPicture(command.SessionId, fileUrl);
            _sessionPictureRepository.Create(sessionPicture);
            _sessionPictureRepository.Save();
            return operation.Succeed();
        }

        public async Task<OperationResult> Edit(EditSessionPicture command)
        {
            var operation = new OperationResult();
            var sessionPicture = _sessionPictureRepository.GetBy(command.Id);
            if (sessionPicture == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            var fileName = await _fileManager.Upload(command.Picture, false);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return operation.Failed(ApplicationMessages.UploadProgressCanceled);
            }
            sessionPicture.Edit(command.SessionId,fileName);
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
