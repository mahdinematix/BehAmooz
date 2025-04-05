using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Class
{
    public class IndexModel : PageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly IClassApplication _classApplication;

        public ClassSearchModel SearchModel;
        public SelectList Courses;
        public List<ClassViewModel> Classes;
        

        public IndexModel(ICourseApplication courseApplication, IClassApplication classApplication)
        {
            _courseApplication = courseApplication;
            _classApplication = classApplication;
        }

        public void OnGet(ClassSearchModel searchModel)
        {
            Courses = new SelectList(_courseApplication.GetCourses(), "Id", "Name");
            Classes = _classApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new CreateClass
            {
                Courses = _courseApplication.GetCourses()
            };
            return Partial("./Create", command);
        }

        public IActionResult OnPostCreate(CreateClass command)
        {
            var result = _classApplication.Create(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var classs = _classApplication.GetDetails(id);
            classs.Courses = _courseApplication.GetCourses();
            return Partial("Edit", classs);
        }

        public IActionResult OnPostEdit(EditClass command)
        {
            var result = _classApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetActivate(long id)
        {
            _classApplication.Activate(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _classApplication.DeActivate(id);
            return RedirectToPage("./Index");
        }
    }
}
