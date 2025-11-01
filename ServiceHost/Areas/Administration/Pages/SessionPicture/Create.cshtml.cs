using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;

namespace ServiceHost.Areas.Administration.Pages.SessionPicture
{
    public class CreateModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IFileManager _fileManager;
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly IAuthHelper _authHelper;
        private readonly IClassApplication _classApplication;
        public CreateSessionPicture Command;
        public SessionViewModel Session;
        public ClassViewModel Class;
        [TempData]
        public string Message { get; set; }
        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, ISessionPictureApplication sessionPictureApplication, IFileManager fileManager, IAuthHelper authHelper)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _sessionPictureApplication = sessionPictureApplication;
            _fileManager = fileManager;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long sessionId)
        {
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(Session.ClassId);
            return Page();
        }

        public IActionResult OnPost(CreateSessionPicture command, long sessionId)
        {
            command.SessionId = sessionId;
            var result = _sessionPictureApplication.CreateAsync(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { sessionId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Create", new { sessionId });
        }

        public async Task<IActionResult> OnGetCancel(long sessionId)
        {

            await _fileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;


            return RedirectToPage("./Create", new { sessionId });
        }


    }
}
