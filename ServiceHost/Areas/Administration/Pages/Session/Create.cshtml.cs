using _01_Framework.Application;
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
        private readonly IFileManager _FileManager;
        public CreateSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _FileManager = fileManager;
        }

        public void OnGet(long classId)
        {
            Class = _classApplication.GetClassById(classId);
        }

        public IActionResult OnPost(CreateSession command, long classId)
        {
            if (ModelState.IsValid)
            {
                command.ClassId = classId;
                var result = _sessionApplication.Create(command);
                if (result.Result.IsSucceeded)
                {
                    return RedirectToPage("./Index", new { classId = classId });
                }

                Message = result.Result.Message;
                return RedirectToPage("./Create", new { classId = classId });
            }
            Message = "·ÿ›« „ﬁ«œ?— —« Ê«—œ ò‰?œ";
            return RedirectToPage("./Create", new { classId = classId });

        }

        public async Task<IActionResult> OnGetCancel(long classId)
        {
            await _FileManager.Cancel();
            Message = "›—«?‰œ ¬Å·Êœ »« „Ê›ﬁ?  ·€Ê ‘œ.";
            return RedirectToPage("./Create", new { classId = classId });
        }
    }
}
