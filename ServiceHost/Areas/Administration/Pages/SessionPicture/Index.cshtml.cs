using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;

namespace ServiceHost.Areas.Administration.Pages.SessionPicture
{
    public class IndexModel : PageModel
    {
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        public SessionViewModel Session;
        public ClassViewModel Class;
        public List<SessionPictureViewModel> SessionPictures;

        public IndexModel(ISessionPictureApplication sessionPictureApplication, ISessionApplication sessionApplication, IClassApplication classApplication)
        {
            _sessionPictureApplication = sessionPictureApplication;
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
        }

        public void OnGet(long sessionId)
        {
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(Session.ClassId);
            SessionPictures = _sessionPictureApplication.GetSessionPicturesBySessionId(sessionId);
        }

        public IActionResult OnGetRemove(long id, long sessionId)
        {
            _sessionPictureApplication.Remove(id);
            return RedirectToPage("./Index", new {sessionId=sessionId});
        }

        public IActionResult OnGetRestore(long id, long sessionId)
        {
            _sessionPictureApplication.Restore(id);
            return RedirectToPage("./Index", new {sessionId=sessionId});
        }
    }
}
