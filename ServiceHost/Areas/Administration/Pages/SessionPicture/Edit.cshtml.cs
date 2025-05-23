using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;

namespace ServiceHost.Areas.Administration.Pages.SessionPicture
{
    public class EditModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly IClassApplication _classApplication;
        public EditSessionPicture Command;
        public SessionViewModel Session;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, ISessionPictureApplication sessionPictureApplication, IClassApplication classApplication)
        {
            _sessionApplication = sessionApplication;
            _sessionPictureApplication = sessionPictureApplication;
            _classApplication = classApplication;
        }

        public void OnGet(long id, long sessionId)
        {
            Command = _sessionPictureApplication.GetDetails(id);
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(Session.ClassId);
        }

        public IActionResult OnPost(EditSessionPicture command, long sessionId)
        {
            command.SessionId = sessionId;
            var result = _sessionPictureApplication.Edit(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { sessionId = sessionId });
            }
            Message = result.Message;
            return RedirectToPage("./Edit", new { sessionId = sessionId });
        }
    }
}
