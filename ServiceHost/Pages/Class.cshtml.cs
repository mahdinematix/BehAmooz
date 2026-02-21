using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class ClassModel : UserContextPageModel
    {
        private readonly IClassQuery _classQuery;
        private readonly ICourseQuery _courseQuery;
        public List<ClassQueryModel> Classes;
        public string Course;

        public ClassModel(IClassQuery classQuery, ICourseQuery courseQuery, IAuthHelper authHelper):base(authHelper)
        {
            _classQuery = classQuery;
            _courseQuery = courseQuery;
        }

        public IActionResult OnGet(long courseId)
        {

            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Classes = _classQuery.GetClassesByCourseId(courseId);
            Course = _courseQuery.GetCourseNameById(courseId);
            return Page();
        }
    }
}
