using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class RejectedModel : PageModel
    {
        private readonly IAuthHelper _authHelper;

        public RejectedModel(IAuthHelper authHelper)
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

            if (status == Statuses.Confirmed && _authHelper.CurrentAccountRole() == Roles.Student)
            {
                return RedirectToPage("/Index");
            }


            if (status == Statuses.Confirmed && _authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }
            return Page();
        }
    }
}
