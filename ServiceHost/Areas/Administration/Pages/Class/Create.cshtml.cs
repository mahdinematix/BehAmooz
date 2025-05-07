using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Class
{
    public class CreateModel : PageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly ICourseApplication _courseApplication;
        public CreateClass Command;
        [TempData] public string Message { get; set; }
        public CourseViewModel Course { get; set; }

        public CreateModel(IClassApplication classApplication, ICourseApplication courseApplication)
        {
            _classApplication = classApplication;
            _courseApplication = courseApplication;
        }

        public void OnGet(long courseId)
        {
            Course = _courseApplication.GetByCourseId(courseId);
        }

        public IActionResult OnPost(CreateClass command, long courseId)
        {
            command.CourseId = courseId;
            var result = _classApplication.Create(command);
            Message = result.Message;
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new {courseId = courseId});
            }
            return RedirectToPage("./Create", new {courseId = courseId});
        }
    }
}
