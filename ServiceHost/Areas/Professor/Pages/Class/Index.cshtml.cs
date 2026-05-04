using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Professor.Pages.Class
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly IClassApplication _classApplication;
        private readonly IAccountApplication _accountApplication;

        public ClassSearchModel SearchModel;
        public List<ClassViewModel> Classes;
        public SelectList Professors;
        public CourseViewModel Course { get; set; }

        public IndexModel(ICourseApplication courseApplication, IClassApplication classApplication, IAccountApplication accountApplication, IAuthHelper authHelper):base(authHelper)
        {
            _courseApplication = courseApplication;
            _classApplication = classApplication;
            _accountApplication = accountApplication;
        }

        public IActionResult OnGet(ClassSearchModel searchModel, long courseId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Classes = _classApplication.Search(searchModel, courseId, CurrentAccountId,CurrentAccountRole);
            Course = _courseApplication.GetById(courseId);
            Professors = new SelectList(_accountApplication.GetProfessors(CurrentAccountRole, CurrentAccountUniversityId), "Id", "FullName");
            return Page();
        }

        public IActionResult OnGetActivate(long id)
        {
            _classApplication.Activate(id,CurrentAccountId);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _classApplication.DeActivate(id,CurrentAccountId);
            return RedirectToPage("./Index");
        }
    }
}
