using _01_Framework.Application;
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
        private readonly IAuthHelper _authHelper;
        public SessionViewModel Session;
        public ClassViewModel Class;
        public List<SessionPictureViewModel> SessionPictures;

        public IndexModel(ISessionPictureApplication sessionPictureApplication, ISessionApplication sessionApplication, IClassApplication classApplication, IAuthHelper authHelper)
        {
            _sessionPictureApplication = sessionPictureApplication;
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
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
            SessionPictures = _sessionPictureApplication.GetSessionPicturesBySessionId(sessionId);
            return Page();
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
