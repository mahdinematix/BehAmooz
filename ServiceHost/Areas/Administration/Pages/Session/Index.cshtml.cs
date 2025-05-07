using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Domain.ClassAgg;

namespace ServiceHost.Areas.Administration.Pages.Session
{
    public class IndexModel : PageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly ISessionApplication _sessionApplication;

        public List<SessionViewModel> Sessions;
        public ClassViewModel Class { get; set; }


        public IndexModel(IClassApplication classApplication, ISessionApplication sessionApplication)
        {
            _classApplication = classApplication;
            _sessionApplication = sessionApplication;
        }

        public void OnGet(long classId)
        {
            Sessions = _sessionApplication.GetAllByClassId(classId);
            Class = _classApplication.GetClassById(classId);
        }
        public IActionResult OnGetActivate(long id, long classId)
        {
            _sessionApplication.Activate(id);
            return RedirectToPage("./Index", new { classId = classId });
        }

        public IActionResult OnGetDeActivate(long id, long classId)
        {
            _sessionApplication.DeActivate(id);
            return RedirectToPage("./Index", new{classId=classId});
        }
    }
}
