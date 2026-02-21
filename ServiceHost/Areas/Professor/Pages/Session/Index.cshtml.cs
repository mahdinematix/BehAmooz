using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Professor.Pages.Session
{
    public class IndexModel : UserContextPageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly ISessionApplication _sessionApplication;

        public List<SessionViewModel> Sessions;
        public ClassViewModel Class { get; set; }


        public IndexModel(IClassApplication classApplication, ISessionApplication sessionApplication, IAuthHelper authHelper):base(authHelper)
        {
            _classApplication = classApplication;
            _sessionApplication = sessionApplication;
        }

        public IActionResult OnGet(long classId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Sessions = _sessionApplication.GetAllByClassId(classId);
            Class = _classApplication.GetClassById(classId);
            return Page();
        }
        public IActionResult OnGetActivate(long id, long classId)
        {
            _sessionApplication.Activate(id, CurrentAccountId);
            return RedirectToPage("./Index", new { classId = classId });
        }

        public IActionResult OnGetDeActivate(long id, long classId)
        {
            _sessionApplication.DeActivate(id, CurrentAccountId);
            return RedirectToPage("./Index", new{classId=classId});
        }
    }
}
