using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;

namespace ServiceHost.Areas.Administration.Pages.SessionPicture
{
    public class EditModel : UserContextPageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IFileManager _fileManager;
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly IClassApplication _classApplication;
        public EditSessionPicture Command;
        public SessionViewModel Session;
        public ClassViewModel Class;
        [BindProperty] public long ClassId { get; set; }
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, ISessionPictureApplication sessionPictureApplication, IClassApplication classApplication, IFileManager fileManager, IAuthHelper authHelper) : base(authHelper)
        {
            _sessionApplication = sessionApplication;
            _sessionPictureApplication = sessionPictureApplication;
            _classApplication = classApplication;
            _fileManager = fileManager;
        }

        public IActionResult OnGet(long id, long sessionId, long classId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            ClassId = classId;
            Command = _sessionPictureApplication.GetDetails(id);
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(classId);
            return Page();
        }

        public IActionResult OnPost(EditSessionPicture command)
        {
            var result = _sessionPictureApplication.Edit(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { sessionId = command.SessionId, classId = ClassId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Edit", new { id = command.Id, sessionId = command.SessionId, classId = ClassId });
        }

        public async Task<IActionResult> OnGetCancel(long sessionId, long id, long classId)
        {
            await _fileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;
            return RedirectToPage("./Edit", new { id, sessionId, classId });
        }

    }
}
