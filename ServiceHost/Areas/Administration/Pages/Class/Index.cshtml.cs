using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using LogManagement.Application.Contracts.Log;
using LogManagement.Infrastructure.Configuration.Permissions;
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
        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;
        private readonly ILogApplication _logApplication;

        public ClassSearchModel SearchModel;
        public List<ClassViewModel> Classes;
        public SelectList Professors;
        public CourseViewModel Course { get; set; }

        public IndexModel(ICourseApplication courseApplication, IClassApplication classApplication, IAccountApplication accountApplication, IAuthHelper authHelper, ILogApplication logApplication)
        {
            _courseApplication = courseApplication;
            _classApplication = classApplication;
            _accountApplication = accountApplication;
            _authHelper = authHelper;
            _logApplication = logApplication;
        }

        public IActionResult OnGet(ClassSearchModel searchModel, long courseId)
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
            Classes = _classApplication.Search(searchModel, courseId);
            Course = _courseApplication.GetByCourseId(courseId);
            Professors = new SelectList(_accountApplication.GetProfessors(), "Id", "FullName");
            return Page();
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

        public IActionResult OnGetCopy(long id, long courseId)
        {
            CopyClass command;
            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                command = new CopyClass
                {
                    Classes = _classApplication.GetClassesForCopy(id, courseId),
                    ClassCode = _classApplication.GetClassCodeById(id),
                    ClassId = id
                };
            }
            else
            {
                command = new CopyClass
                {
                    Classes = _classApplication.GetClasses(id),
                    ClassCode = _classApplication.GetClassCodeById(id),
                    ClassId = id
                };
            }
            
            return Partial("Copy", command);
        }

        public IActionResult OnPostCopy(CopyClass command)
        {
            var result = _classApplication.Copy(command);
            return new JsonResult(result);
        }

        [NeedsPermissions(LogPermissions.ShowLogs)]
        public IActionResult OnGetLogs(long id)
        {
            var logs = _logApplication.GetClassLogsById(id);
            return Partial("./Logs", logs);
        }
    }
}
