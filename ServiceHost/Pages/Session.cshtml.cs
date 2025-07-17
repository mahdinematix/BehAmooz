using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class SessionModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly ISessionQuery _sessionQuery;
        public SessionQueryModel Session;

        public SessionModel(IAuthHelper authHelper, ISessionQuery sessionQuery)
        {
            _authHelper = authHelper;
            _sessionQuery = sessionQuery;
        }

        public IActionResult OnGet(long sessionId)
        {
            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }

            Session = _sessionQuery.GetSessionById(sessionId);

            return Page();
        }
    }
}
