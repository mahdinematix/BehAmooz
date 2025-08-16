using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class SessionModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly ISessionQuery _sessionQuery;
        private readonly IClassQuery _classQuery;
        public SessionQueryModel Session;
        public ClassQueryModel Class;
        public CourseQueryModel Course;

        public SessionModel(IAuthHelper authHelper, ISessionQuery sessionQuery, IClassQuery classQuery)
        {
            _authHelper = authHelper;
            _sessionQuery = sessionQuery;
            _classQuery = classQuery;
        }

        public IActionResult OnGet(long sessionId)
        {
            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Rejected");
            }

            Session = _sessionQuery.GetSessionById(sessionId);
            Class = _classQuery.GetClassById(Session.ClassId);
            Course = _classQuery.GetCourseNameAndPriceByClassId(Class.CourseId);
            return Page();
        }
    }
}
