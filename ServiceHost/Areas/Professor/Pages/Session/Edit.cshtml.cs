using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Professor.Pages.Session
{
    public class EditModel : UserContextPageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        private readonly IFileManager _fileManager;
        public EditSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager, IAuthHelper authHelper):base(authHelper)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _fileManager = fileManager;
        }

        public IActionResult OnGet(long id, long classId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Command = _sessionApplication.GetDetails(id);
            Class = _classApplication.GetClassById(classId);
            return Page();
        }

        public IActionResult OnPost(EditSession command, long classId)
        {
            command.ClassId = classId;
            var result = _sessionApplication.Edit(command, CurrentAccountId);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { classId = classId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Edit", new { classId = classId });
        }

        public async Task<IActionResult> OnGetCancel(long classId , long id)
        {
            await _fileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;
            return RedirectToPage("./Edit", new {classId , id });
        }
    }
}
