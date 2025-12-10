using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using _02_Query.Contracts.Order;
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
        private readonly IOrderQuery _orderQuery;
        public SessionQueryModel Session;
        public ClassQueryModel Class;
        public CourseQueryModel Course;
        public bool IsPaid;

        public SessionModel(IAuthHelper authHelper, ISessionQuery sessionQuery, IClassQuery classQuery, IOrderQuery orderQuery)
        {
            _authHelper = authHelper;
            _sessionQuery = sessionQuery;
            _classQuery = classQuery;
            _orderQuery = orderQuery;
        }

        public IActionResult OnGet(long sessionId)
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

            IsPaid = _orderQuery.IsPaid(sessionId);
            Session = _sessionQuery.GetSessionById(sessionId);
            Class = _classQuery.GetClassById(Session.ClassId);
            Course = _classQuery.GetCourseNameAndPriceByClassId(Class.CourseId);
            return Page();
        }
    }
}
