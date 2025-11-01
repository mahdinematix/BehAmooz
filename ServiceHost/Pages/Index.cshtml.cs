using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly ICourseQuery _courseQuery;
        public List<CourseQueryModel> Courses { get; set; }
        public CourseSearchModel SearchModel;



        public IndexModel(IAuthHelper authHelper, ICourseQuery courseQuery)
        {
            _authHelper = authHelper;
            _courseQuery = courseQuery;
        }

        public IActionResult OnGet(CourseSearchModel searchModel)
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

            Courses = _courseQuery.Search(searchModel);
            

            return Page();
        }
    }
}
