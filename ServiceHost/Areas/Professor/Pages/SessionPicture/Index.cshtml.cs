using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Application.Contracts.SessionPicture;

namespace ServiceHost.Areas.Professor.Pages.SessionPicture
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ISessionPictureApplication _sessionPictureApplication;
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        public SessionViewModel Session;
        public ClassViewModel Class;
        public List<SessionPictureViewModel> SessionPictures;

        public IndexModel(ISessionPictureApplication sessionPictureApplication, ISessionApplication sessionApplication, IClassApplication classApplication, IAuthHelper authHelper) : base(authHelper)
        {
            _sessionPictureApplication = sessionPictureApplication;
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
        }

        public IActionResult OnGet(long sessionId, long classId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Session = _sessionApplication.GetBySessionId(sessionId);
            Class = _classApplication.GetClassById(classId);
            SessionPictures = _sessionPictureApplication.GetSessionPicturesBySessionId(sessionId);
            return Page();
        }

        public IActionResult OnGetRemove(long id, long sessionId, long classId)
        {
            _sessionPictureApplication.Remove(id);
            return RedirectToPage("./Index", new { sessionId, classId });
        }

        public IActionResult OnGetRestore(long id, long sessionId, long classId)
        {
            _sessionPictureApplication.Restore(id);
            return RedirectToPage("./Index", new { sessionId, classId });
        }
    }
}
