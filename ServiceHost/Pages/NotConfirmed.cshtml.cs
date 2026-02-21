using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class NotConfirmedModel : UserContextPageModel
    {

        public NotConfirmedModel(IAuthHelper authHelper):base(authHelper)
        {
        }

        public IActionResult OnGet()
        {
            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            if (CurrentAccountStatus == Statuses.Confirmed && CurrentAccountRole== Roles.Student)
            {
                return RedirectToPage("/Index");
            }


            if (CurrentAccountStatus == Statuses.Confirmed && CurrentAccountRole != Roles.Student)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }
            return Page();
        }
    }
}
