using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Professor.Pages.Session
{
    public class CreateModel : UserContextPageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        private readonly IFileManager _FileManager;
        public CreateSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public CreateModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager, IAuthHelper authHelper):base(authHelper)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _FileManager = fileManager;
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
            Class = _classApplication.GetClassById(classId);
            return Page();
        }

        public async Task<IActionResult> OnPost(CreateSession command, long classId)
        {
            if (ModelState.IsValid)
            {
                command.ClassId = classId;
                var result = await _sessionApplication.Create(command, CurrentAccountId);
                if (result.IsSucceeded)
                {
                    return RedirectToPage("./Index", new { classId = classId });
                }

                Message = result.Message;
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
