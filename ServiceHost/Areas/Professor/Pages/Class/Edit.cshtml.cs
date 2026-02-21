using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Professor.Pages.Class
{
    public class EditModel : UserContextPageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly ICourseApplication _courseApplication;
        [TempData] public string Message { get; set; }
        public EditClass Command;
        public CourseViewModel Course { get; set; }
        public long ProfessorId { get; set; }


        public EditModel(IClassApplication classApplication, IAccountApplication accountApplication, ICourseApplication courseApplication, IAuthHelper authHelper):base(authHelper)
        {
            _classApplication = classApplication;
            _accountApplication = accountApplication;
            _courseApplication = courseApplication;
        }

        public IActionResult OnGet(long id, long courseId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            ProfessorId = CurrentAccountId;
            Command = _classApplication.GetDetails(id);
            Course = _courseApplication.GetByCourseId(courseId);
            return Page();
        }

        public IActionResult OnPost(EditClass command, long courseId)
        {
            command.CourseId = courseId;
            var result = _classApplication.Edit(command,CurrentAccountId);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { courseId = courseId });
            }
            Message = result.Message;
            return RedirectToPage("./Edit", new { courseId = courseId, id = command.Id });
        }
    }
}
