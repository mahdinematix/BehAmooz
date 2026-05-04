using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Administration.Pages.Session
{
    public class EditModel : UserContextPageModel
    {
        private readonly ISessionApplication _sessionApplication;
        private readonly IClassApplication _classApplication;
        private readonly IFileManager _fileManager;
        public EditSession Command;
        public ClassViewModel Class;
        public long ClassTemplateId { get; set; }

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
            ClassTemplateId = _classApplication.GetTemplateIdByClassId(classId);

            return Page();
        }

        public IActionResult OnPost(EditSession command, long classId)
        {
            var templateId = _classApplication.GetTemplateIdByClassId(classId);
            command.ClassTemplateId = templateId;
            var result = _sessionApplication.Edit(command, CurrentAccountId);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { classId });
            }
            Message = result.Result.Message;
            return RedirectToPage("./Edit", new { classId });
        }

        public async Task<IActionResult> OnGetCancel(long classId , long id)
        {
            await _fileManager.Cancel();
            Message = ApplicationMessages.UploadProgressCanceled;
            return RedirectToPage("./Edit", new {classId , id });
        }
    }
}
