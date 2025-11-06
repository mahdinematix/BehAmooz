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
        private readonly IAuthHelper _authHelper;
        public CreateSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager, IAuthHelper authHelper)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _FileManager = fileManager;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long classId)
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
            Class = _classApplication.GetClassById(classId);
            return Page();
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
            Message = ApplicationMessages.FillTheForms;
            return RedirectToPage("./Create", new { classId = classId });

        }

        public async Task<IActionResult> OnGetCancel(long classId)
        { 
            await _FileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;
            return RedirectToPage("./Create", new { classId });
        }
    }
}
