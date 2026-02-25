using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.LogContracts;
using LogManagement.Infrastructure.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly ILogApplication _logApplication;
        private readonly ISemesterApplication _semesterApplication;
        private readonly IUniversityApplication _universityApplication;

        public CourseSearchModel SearchModel;
        public List<CourseViewModel> Courses;
        public SemesterViewModel CurrentSemester;
        public SelectList SemesterCodes;
        public string UniversityName;
        [BindProperty] public long UniversityId { get; set; }

        public IndexModel(ICourseApplication courseApplication, IAuthHelper authHelper, ILogApplication logApplication, ISemesterApplication semesterApplication, IUniversityApplication universityApplication):base(authHelper)
        {
            _courseApplication = courseApplication;
            _logApplication = logApplication;
            _semesterApplication = semesterApplication;
            _universityApplication = universityApplication;
        }


        public IActionResult OnGet(CourseSearchModel searchModel,long universityId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            if (CurrentAccountRole == Roles.Administrator)
            {
                if (universityId != CurrentAccountUniversityId)
                {
                    return RedirectToPage("./Index" ,new {universityId = CurrentAccountUniversityId});
                }
            }
            CurrentSemester = _semesterApplication.GetCurrentSemester(universityId);

            SemesterCodes = new SelectList(
                _semesterApplication.GetSemestersByUniversityId(universityId), 
                "Id",                           
                "Code"               
            );
            UniversityId = universityId;
            UniversityName = _universityApplication.GetNameBy(universityId);
            Courses = _courseApplication.Search(searchModel, universityId, CurrentAccountId, CurrentAccountRole);
            return Page();
        }

        public IActionResult OnGetActivate(long id)
        {
            _courseApplication.Activate(id,CurrentAccountId);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _courseApplication.DeActivate(id, CurrentAccountId);
            return RedirectToPage("./Index");
        }

        [NeedsPermissions(ActivityLogPermissions.ShowActivityLog)]
        public IActionResult OnGetLogs(long id)
        {
            var logs = _logApplication.GetCourseLogsById(id);
            return Partial("./Logs", logs);
        }

        public IActionResult OnGetDefineSemester()
        {
            var command = new DefineSemester();
            return Partial("DefineSemester", command);
        }

        public IActionResult OnPostDefineSemester(DefineSemester command)
        {
            command.UniversityId = UniversityId;
            var result = _semesterApplication.Define(command, CurrentAccountId);
            return new JsonResult(result);
        }
    }
}
