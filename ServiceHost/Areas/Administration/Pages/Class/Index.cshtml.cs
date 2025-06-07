using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Class
{
    public class IndexModel : PageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly IClassApplication _classApplication;
        private readonly IAccountApplication _accountApplication;

        public ClassSearchModel SearchModel;
        public List<ClassViewModel> Classes;
        public CourseViewModel Course { get; set; }
        

        public IndexModel(ICourseApplication courseApplication, IClassApplication classApplication, IAccountApplication accountApplication)
        {
            _courseApplication = courseApplication;
            _classApplication = classApplication;
            _accountApplication = accountApplication;
        }

        public void OnGet(ClassSearchModel searchModel, long courseId)
        {
            Classes = _classApplication.Search(searchModel, courseId);
            Course = _courseApplication.GetByCourseId(courseId);
        }

        public IActionResult OnGetEdit(long id)
        {
            var classs = _classApplication.GetDetails(id);
            
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
