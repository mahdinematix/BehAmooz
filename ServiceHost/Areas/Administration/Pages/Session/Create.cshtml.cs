using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Administration.Pages.Session
{
    public class CreateModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        public CreateSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
        }

        public void OnGet(long classId)
        {
            Class = _classApplication.GetClassById(classId);
        }

        public IActionResult OnPost(CreateSession command, long classId)
        {
            command.ClassId = classId;
            var result = _sessionApplication.Create(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { classId = classId });
            }
            else
            {
                Message = result.Message;
                return RedirectToPage("./Create", new { classId = classId });
            }
                
        }
    }
}
