using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IAuthHelper _authHelper;

        public IndexModel(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            return Page();
        }
    }
}
