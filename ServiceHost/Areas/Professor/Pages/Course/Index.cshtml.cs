using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Professor.Pages.Course
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly ISemesterApplication _semesterApplication;
        private readonly IUniversityApplication _universityApplication;

        public CourseSearchModel SearchModel;
        public List<CourseViewModel> Courses;
        public SemesterViewModel CurrentSemester;
        public SelectList SemesterCodes;
        public string UniversityName;
        public long UniversityId { get; set; }

        public IndexModel(ICourseApplication courseApplication, IAuthHelper authHelper, ISemesterApplication semesterApplication, IUniversityApplication universityApplication):base(authHelper)
        {
            _courseApplication = courseApplication;
            _semesterApplication = semesterApplication;
            _universityApplication = universityApplication;
        }


        public IActionResult OnGet(CourseSearchModel searchModel, long universityId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            UniversityId = universityId;
            CurrentSemester = _semesterApplication.GetCurrentSemester(universityId);
            if (searchModel.SemesterId == 0)
                searchModel.SemesterId = CurrentSemester?.Id ?? 0;

            SemesterCodes = new SelectList(
                _semesterApplication.GetSemestersByUniversityId(universityId), 
                "Id",                           
                "Code"               
            );
            Courses = _courseApplication.Search(searchModel, universityId,CurrentAccountId,CurrentAccountRole);
            UniversityName = _universityApplication.GetNameBy(universityId);
            return Page();
        }

        public IActionResult OnGetActivate(long id)
        {
            _courseApplication.Activate(id, CurrentAccountId);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _courseApplication.DeActivate(id, CurrentAccountId);
            return RedirectToPage("./Index");
        }
    }
}
