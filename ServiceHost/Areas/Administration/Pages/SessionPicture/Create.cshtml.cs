using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;
using StudyManagement.Domain.ClassAgg;

namespace ServiceHost.Areas.Administration.Pages.SessionPicture
{
    public class CreateModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly IClassApplication _classApplication;
        public CreateSessionPicture Command;
        public SessionViewModel Session;
        public ClassViewModel Class;
        [TempData]
        public string Message { get; set; }
        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, ISessionPictureApplication sessionPictureApplication)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _sessionPictureApplication = sessionPictureApplication;
        }

        public void OnGet(long sessionId)
        {
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(Session.ClassId);
        }

        public IActionResult OnPost(CreateSessionPicture command, long sessionId)
        {
            command.SessionId = sessionId;
            var result = _sessionPictureApplication.Create(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { sessionId = sessionId });
            }
            Message = result.Message;
            return RedirectToPage("./Create", new { sessionId = sessionId });
        }

    }
}
