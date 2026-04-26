using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.Session;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class SessionsModel : UserContextPageModel
    {
        private readonly ISessionQuery _sessionQuery;
        private readonly IClassQuery _classQuery;
        private readonly ICourseQuery _courseQuery;
        public ClassQueryModel Class;
        public List<CartItem> Sessions;
        public CourseQueryModel Course { get; set; }

        public SessionsModel(IAuthHelper authHelper, ISessionQuery sessionQuery, IClassQuery classQuery, ICourseQuery courseQuery) : base(authHelper)
        {
            _sessionQuery = sessionQuery;
            _classQuery = classQuery;
            _courseQuery = courseQuery;
        }


        public IActionResult OnGet(long classId)
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

            Sessions = _sessionQuery.GetItemsByClassId(classId);
            Class = _classQuery.GetClassById(classId);
            Course = _courseQuery.GetCourseNameAndPriceById(Class.CourseId);
            return Page();
        }
    }
}
