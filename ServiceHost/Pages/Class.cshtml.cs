using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ClassModel : PageModel
    {
        private readonly IClassQuery _classQuery;
        private readonly ICourseQuery _courseQuery;
        private readonly IAuthHelper _authHelper;
        public List<ClassQueryModel> Classes;
        public string Course;

        public ClassModel(IClassQuery classQuery, ICourseQuery courseQuery, IAuthHelper authHelper)
        {
            _classQuery = classQuery;
            _courseQuery = courseQuery;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long courseId)
        {
            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }
            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Classes = _classQuery.GetClassesByCourseId(courseId);
            Course = _courseQuery.GetCourseNameById(courseId);
            return Page();
        }
    }
}
