using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Professor.Pages.Class
{
    public class CreateModel : UserContextPageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly ICourseApplication _courseApplication;
        public CreateClass Command;
        public long ProfessorId { get; set; }
        [TempData] public string Message { get; set; }
        public CourseViewModel Course { get; set; }

        public CreateModel(IClassApplication classApplication, ICourseApplication courseApplication, IAuthHelper authHelper):base(authHelper)
        {
            _classApplication = classApplication;
            _courseApplication = courseApplication;
        }

        public IActionResult OnGet(long courseId)
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
            Course = _courseApplication.GetByCourseId(courseId);
            return Page();
        }

        public IActionResult OnPost(CreateClass command, long courseId)
        {
            command.CourseId = courseId;
            var result = _classApplication.Create(command,CurrentAccountId);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new {courseId = courseId});
            }
            Message = result.Message;
            return RedirectToPage("./Create", new {courseId = courseId});
        }
    }
}
