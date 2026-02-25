using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Application.Contracts.Session;

namespace ServiceHost.Areas.Professor.Pages.Class
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly IClassApplication _classApplication;
        private readonly IAccountApplication _accountApplication;
        private readonly ISessionApplication _sessionApplication;

        public ClassSearchModel SearchModel;
        public List<ClassViewModel> Classes;
        public SelectList Professors;
        public CourseViewModel Course { get; set; }

        public IndexModel(ICourseApplication courseApplication, IClassApplication classApplication, IAccountApplication accountApplication, IAuthHelper authHelper, ISessionApplication sessionApplication):base(authHelper)
        {
            _courseApplication = courseApplication;
            _classApplication = classApplication;
            _accountApplication = accountApplication;
            _sessionApplication = sessionApplication;
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
            Classes = _classApplication.Search(searchModel, courseId);
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

        public IActionResult OnGetCopyCheck(long id)
        {
            var hasSessions = _sessionApplication.HasAnySessionsByClassId(id);
            if (!hasSessions)
                return new JsonResult(new { isSucceeded = false, message = ApplicationMessages.TheClassHasNotAnySessions });

            return new JsonResult(new { isSucceeded = true });
        }
        public IActionResult OnGetCopy(long id)
        {
            var command = new CopyClass
            {
                Classes = _classApplication.GetClassesForCopy(id),
                ClassCode = _classApplication.GetClassCodeById(id),
                ClassId = id
            };
            return Partial("Copy", command);
        }

        public IActionResult OnPostCopy(CopyClass command)
        {
            var result = _classApplication.Copy(command,CurrentAccountId);
            return new JsonResult(result);
        }

        public IActionResult OnGetClassInfoByCode(string classCode)
        {
            if (string.IsNullOrWhiteSpace(classCode) || classCode == "0")
                return new JsonResult(new { isSucceeded = false });

            var info = _classApplication.GetClassInfoByClassCode(classCode);

            if (info == null)
                return new JsonResult(new { isSucceeded = false });

            return new JsonResult(new
            {
                isSucceeded = true,
                courseName = info.CourseName,
                day = info.Day,
                startTime = info.StartTime,
                endTime = info.EndTime
            });
        }


    }
}
