using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class NotConfirmedModel : PageModel
    {
        private readonly IAuthHelper _authHelper;

        public NotConfirmedModel(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {
            StatusChecker();
            return Page();
        }

        private IActionResult StatusChecker()
        {
            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Rejected");
            }

            if (status == Statuses.Confirmed)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }
    }
}
