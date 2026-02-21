using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Course;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ICourseQuery _courseQuery;
        public List<CourseQueryModel> Courses { get; set; }
        public CourseSearchModel SearchModel;

        public IndexModel(IAuthHelper authHelper, ICourseQuery courseQuery):base(authHelper)
        {
            _courseQuery = courseQuery;
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

            Courses = _courseQuery.Search(searchModel);
            
            return Page();
        }
    }
}
