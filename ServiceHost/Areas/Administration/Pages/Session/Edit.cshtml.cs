using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Administration.Pages.Session
{
    public class EditModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        public EditSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, IClassApplication classApplication)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
        }

        public void OnGet(long id, long classId)
        {
            Command = _sessionApplication.GetDetails(id);
            Class = _classApplication.GetClassById(classId);
        }

        public IActionResult OnPost(EditSession command, long classId)
        {
            command.ClassId = classId;
            var result = _sessionApplication.Edit(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { classId = classId });
            }
            Message = result.Message;
            return RedirectToPage("./Edit", new { classId = classId });
        }
    }
}
