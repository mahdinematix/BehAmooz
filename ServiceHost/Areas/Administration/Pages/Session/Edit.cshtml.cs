using _01_Framework.Application;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;
using System.Runtime.CompilerServices;

namespace ServiceHost.Areas.Administration.Pages.Session
{
    public class EditModel : PageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        private readonly IFileManager _fileManager;
        private readonly IAuthHelper _authHelper;
        public EditSession Command;
        public ClassViewModel Class;
        [TempData] public string Message { get; set; }

        public EditModel(ISessionApplication sessionApplication, IClassApplication classApplication, IFileManager fileManager, IAuthHelper authHelper)
        {
            _sessionApplication = sessionApplication;
            _classApplication = classApplication;
            _fileManager = fileManager;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long id, long classId)
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
            Command = _sessionApplication.GetDetails(id);
            Class = _classApplication.GetClassById(classId);
            return Page();
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

        public async Task<IActionResult> OnGetCancel(long classId , long id)
        {
            await _fileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;
            return RedirectToPage("./Edit", new {classId , id });
        }
    }
}
