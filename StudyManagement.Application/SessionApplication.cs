using _01_Framework.Application;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Application
{
    public class SessionApplication : ISessionApplication
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IFileManager _fileManager;

        public SessionApplication(ISessionRepository sessionRepository, IFileManager fileManager)
        {
            _sessionRepository = sessionRepository;
            _fileManager = fileManager;
        }

        public async Task<OperationResult> Create(CreateSession command)
        {
            var operation = new OperationResult();
            if (_sessionRepository.Exists(x=>x.Number == command.Number && x.ClassId == command.ClassId))
            {
                return operation.Failed(ApplicationMessages.ASessionWithThatNumberExists);
            }
            string fileUrlForVideo = "";
            string fileUrlForBooklet = "";

            if (command.Video != null)
                fileUrlForVideo = await _fileManager.Upload(command.Video, true);

            if (command.Booklet != null)
                fileUrlForBooklet = await _fileManager.Upload(command.Booklet, false);
            bool videoCanceledOrInvalid = command.Video == null &&
                                          (string.IsNullOrWhiteSpace(fileUrlForVideo) || !fileUrlForVideo.StartsWith("http", StringComparison.OrdinalIgnoreCase));

            bool bookletCanceledOrInvalid = command.Booklet != null &&
                                            (string.IsNullOrWhiteSpace(fileUrlForBooklet) || !fileUrlForBooklet.StartsWith("http", StringComparison.OrdinalIgnoreCase));

            if (videoCanceledOrInvalid || bookletCanceledOrInvalid)
                return operation.Failed(ApplicationMessages.UploadProgressCanceled);

            bool videoCanceled = command.Video != null && string.IsNullOrWhiteSpace(fileUrlForVideo);
            bool bookletCanceled = command.Booklet != null && string.IsNullOrWhiteSpace(fileUrlForBooklet);

            if (videoCanceled || bookletCanceled)
                return operation.Failed(ApplicationMessages.UploadProgressCanceled);


            var session = new Session(command.Number, command.Title, fileUrlForVideo, fileUrlForBooklet,
                command.Description, command.ClassId);
            _sessionRepository.Create(session);
            _sessionRepository.Save();
            return operation.Succeed();
        }

        public async Task<OperationResult> Edit(EditSession command)
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
            string fileUrlForVideo = "";
            string fileUrlForBooklet = "";

            if (command.Video != null)
                fileUrlForVideo = await _fileManager.Upload(command.Video, true);

            if (command.Booklet != null)
                fileUrlForBooklet = await _fileManager.Upload(command.Booklet, false);

            bool videoCanceled = command.Video != null && string.IsNullOrWhiteSpace(fileUrlForVideo);
            bool bookletCanceled = command.Booklet != null && string.IsNullOrWhiteSpace(fileUrlForBooklet);

            if (videoCanceled || bookletCanceled)
                return operation.Failed(ApplicationMessages.UploadProgressCanceled);

            session.Edit(command.Number, command.Title, fileUrlForVideo, fileUrlForBooklet,
                command.Description, command.ClassId);
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
