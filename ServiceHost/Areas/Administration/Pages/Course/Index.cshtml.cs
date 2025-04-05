using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class IndexModel : PageModel
    {
        private readonly ICourseApplication _courseApplication;

        public CourseSearchModel SearchModel;
        public List<CourseViewModel> Courses;
        

        public IndexModel(ICourseApplication courseApplication)
        {
            _courseApplication = courseApplication;
        }

        public void OnGet(CourseSearchModel searchModel)
        {
            Courses = _courseApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            return Partial("./Create", new CreateCourse());
        }

        public IActionResult OnPostCreate(CreateCourse command)
        {
            var result = _courseApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var course = _courseApplication.GetDetails(id);
            return Partial("Edit", course);
        }

        public IActionResult OnPostEdit(EditCourse command)
        {
            var result = _courseApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetActivate(long id)
        {
            _courseApplication.Activate(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _courseApplication.DeActivate(id);
            return RedirectToPage("./Index");
        }
    }
}
