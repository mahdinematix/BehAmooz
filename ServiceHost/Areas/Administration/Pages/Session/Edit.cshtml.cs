using _01_Framework.Application;
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
        private readonly IFileManager _fileManager;
        public EditSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _fileManager = fileManager;
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
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { classId = classId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Edit", new { classId = classId });
        }

        public async Task<IActionResult> OnGetCancel(long classId)
        {
            await _fileManager.Cancel();
            Message = "›—«?‰œ ¬Å·Êœ »« „Ê›ﬁ?  ·€Ê ‘œ.";
            return RedirectToPage("./Create", new { classId = classId });
        }
    }
}
