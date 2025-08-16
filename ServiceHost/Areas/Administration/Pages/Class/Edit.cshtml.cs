using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Class
{
    public class EditModel : PageModel
    {
        private readonly IClassApplication _classApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly ICourseApplication _courseApplication;
        [TempData] public string Message { get; set; }
        public EditClass Command;
        public SelectList Professors;
        public CourseViewModel Course { get; set; }

        public EditModel(IClassApplication classApplication, IAccountApplication accountApplication, ICourseApplication courseApplication)
        {
            _classApplication = classApplication;
            _accountApplication = accountApplication;
            _courseApplication = courseApplication;
        }

        public void OnGet(long id, long courseId)
        {
            Professors = new SelectList(_accountApplication.GetProfessors(), "Id", "FullName");
            Command = _classApplication.GetDetails(id);
            Course = _courseApplication.GetByCourseId(courseId);
        }

        public IActionResult OnPost(EditClass command, long courseId)
        {
            command.CourseId = courseId;
            var result = _classApplication.Edit(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index", new { courseId = courseId });
            }
            Message = result.Message;
            return RedirectToPage("./Edit", new { courseId = courseId, id = command.Id });
        }
    }
}
