using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class SessionsModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly ISessionQuery _sessionQuery;
        private readonly IClassQuery _classQuery;
        public ClassQueryModel Class;
        public List<CartItem> Sessions;
        public string CourseName { get; set; }

        public SessionsModel(IAuthHelper authHelper, ISessionQuery sessionQuery, IClassQuery classQuery)
        {
            _authHelper = authHelper;
            _sessionQuery = sessionQuery;
            _classQuery = classQuery;
        }


        public IActionResult OnGet(long classId)
        {

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            Sessions = _sessionQuery.GetItemsByClassId(classId);
            Class = _classQuery.GetClassById(classId);
            CourseName = _classQuery.GetCourseNameByClassId(classId);

            return Page();
        }
    }
}
