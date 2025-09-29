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
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly IClassApplication _classApplication;
        private readonly IFileManager _FileManager;
        public CreateSessionPicture Command;
        public SessionViewModel Session;
        public ClassViewModel Class;
        [TempData]
        public string Message { get; set; }
        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, ISessionPictureApplication sessionPictureApplication, IFileManager fileManager)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _sessionPictureApplication = sessionPictureApplication;
            _FileManager = fileManager;
        }

        public void OnGet(long sessionId)
        {
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(Session.ClassId);
        }

        public IActionResult OnPost(CreateSessionPicture command, long sessionId)
        {
            command.SessionId = sessionId; 
            var result = _sessionPictureApplication.CreateAsync(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { sessionId = sessionId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Create", new { sessionId = sessionId });
        }

        public async Task<IActionResult> OnGetCancel(long classId)
        {
            await _FileManager.Cancel();
            Message = "›—«?‰œ ¬Å·Êœ »« „Ê›ﬁ?  ·€Ê ‘œ.";
            return RedirectToPage("./Create", new { classId = classId });
        }
    }
}
