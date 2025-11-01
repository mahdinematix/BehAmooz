using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Class
{
    public class CreateModel : PageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly ICourseApplication _courseApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;
        public CreateClass Command;
        public SelectList Professors;
        [TempData] public string Message { get; set; }
        public CourseViewModel Course { get; set; }

        public CreateModel(IClassApplication classApplication, ICourseApplication courseApplication, IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _classApplication = classApplication;
            _courseApplication = courseApplication;
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long courseId)
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
            Course = _courseApplication.GetByCourseId(courseId);
            Professors = new SelectList(_accountApplication.GetProfessors(), "Id", "FullName");
            return Page();
        }

        public IActionResult OnPost(CreateClass command, long courseId)
        {
            command.CourseId = courseId;
            var result = _classApplication.Create(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new {courseId = courseId});
            }
            Message = result.Message;
            return RedirectToPage("./Create", new {courseId = courseId});
        }
    }
}
