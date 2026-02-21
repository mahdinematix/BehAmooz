using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.LogContracts;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Application
{
    public class SessionApplication : ISessionApplication
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IFileManager _fileManager;
        private readonly ILogApplication _logApplication;

        public SessionApplication(ISessionRepository sessionRepository, IFileManager fileManager, ILogApplication logApplication)
        {
            _sessionRepository = sessionRepository;
            _fileManager = fileManager;
            _logApplication = logApplication;
        }

        public async Task<OperationResult> Create(CreateSession command, long currentAccountId)
        {
            var operation = new OperationResult();
            if (_sessionRepository.Exists(x => x.Number == command.Number && x.ClassId == command.ClassId))
            {
                return operation.Failed(ApplicationMessages.ASessionWithThatNumberExists);
            }
            string fileUrlForVideo = "";
            string fileUrlForBooklet = "";

            if (command.Video != null)
                fileUrlForVideo = await _fileManager.Upload(command.Video, true);

            if (command.Booklet != null)
                fileUrlForBooklet = await _fileManager.Upload(command.Booklet, false);
            bool videoCanceledOrInvalid = command.Video != null &&
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
            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Create,
                TargetId = session.Id,
                TargetType = TargetTypes.Session
            });
            return operation.Succeed();
        }

        public async Task<OperationResult> Edit(EditSession command, long currentAccountId)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(command.Id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_sessionRepository.Exists(x => x.Number == command.Number && x.Id != command.Id && x.ClassId == command.ClassId))
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

            var oldNumber = session.Number;
            var oldTitle = session.Title;
            var oldVideoUrl = fileUrlForVideo;
            var oldBookletUrl = fileUrlForBooklet;
            var oldDescription = session.Description;
            var oldDesc = string.IsNullOrWhiteSpace(oldDescription)
                ? string.Empty
                : oldDescription.Trim();

            var newDesc = string.IsNullOrWhiteSpace(command.Description)
                ? string.Empty
                : command.Description.Trim();

            session.Edit(command.Number, command.Title, fileUrlForVideo, fileUrlForBooklet,
                command.Description, command.ClassId);
            _sessionRepository.Save();

            if (!(oldNumber == command.Number && oldTitle == command.Title && oldVideoUrl == fileUrlForVideo && oldBookletUrl == fileUrlForBooklet && oldDesc == newDesc))
            {
                var changes = new List<string>();


                if (oldNumber != command.Number)
                    changes.Add($"شماره از «{oldNumber}» به «{command.Number}»");

                if (oldTitle != command.Title)
                    changes.Add($"عنوان از «{oldTitle}» به «{command.Title}»");

                if (oldVideoUrl != fileUrlForVideo)
                    changes.Add($"ویدیوی جلسه");

                if (oldBookletUrl != fileUrlForBooklet)
                    changes.Add($"جزوه جلسه");



                if (oldDesc != newDesc)
                {
                    changes.Add($"توضیحات از «{oldDesc}» به «{newDesc}»");
                }


                var description = string.Join(" | ", changes);

                _logApplication.Create(new CreateLog
                {
                    AccountId = currentAccountId,
                    Operation = Operations.Edit,
                    TargetId = session.Id,
                    TargetType = TargetTypes.Session,
                    Description = description
                });
            }
            return operation.Succeed();
        }

        public OperationResult Activate(long id, long currentAccountId)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            session.Activate();
            _sessionRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Activate,
                TargetId = session.Id,
                TargetType = TargetTypes.Session,
            });
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id, long currentAccountId)
        {
            var operation = new OperationResult();
            var session = _sessionRepository.GetBy(id);
            if (session == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            session.DeActivate();
            _sessionRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Deactivate,
                TargetId = session.Id,
                TargetType = TargetTypes.Session,
            });
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

        public bool HasAnySessionsByClassId(long classId)
        {
            return _sessionRepository.HasAnySessionsByClassId(classId);
        }
    }
}
