using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.University;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseQuery _courseQuery;
        private readonly IUniversityQuery _universityQuery;

        public List<CourseQueryModel> Courses { get; set; }
        public CourseSearchModel SearchModel;
        public string UniversityName { get; set; }
        public int UniversityActiveSemester { get; set; }
        public bool IsSuperAdministrator { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsStudent { get; set; }
        public string EducationLvl { get; private set; }
        public string Major { get; set; }

        public IndexModel(IAuthHelper authHelper, ICourseQuery courseQuery, IUniversityQuery universityQuery) : base(authHelper)
        {
            _courseQuery = courseQuery;
            _universityQuery = universityQuery;
        }

        public IActionResult OnGet(CourseSearchModel searchModel)
        {

            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Professor" });
            }
            if (CurrentAccountRole == Roles.Administrator || CurrentAccountRole == Roles.SuperAdministrator)
            {
                var redirectedBefore = TempData.Peek("AdminFirstRedirect");

                if (redirectedBefore == null)
                {
                    TempData["AdminFirstRedirect"] = true;
                    return RedirectToPage("/Index", new { area = "Administration" });
                }
            }

            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            IsSuperAdministrator = CurrentAccountRole == Roles.SuperAdministrator;
            IsAdministrator = CurrentAccountRole == Roles.Administrator;
            IsStudent = CurrentAccountRole == Roles.Student;
            if (IsStudent)
            {
                Major = Majors.GetName(CurrentAccountInfo.MajorId);
                EducationLvl = EducationLevels.GetEducationLevelById(CurrentAccountInfo.EducationLevel);
            }

            UniversityName = _universityQuery.GetById(CurrentAccountUniversityId).Name;
            UniversityActiveSemester = _universityQuery.GetById(CurrentAccountUniversityId).SemesterCode;


            Courses = _courseQuery.Search(searchModel, CurrentAccountInfo);

            return Page();
        }
    }
}
