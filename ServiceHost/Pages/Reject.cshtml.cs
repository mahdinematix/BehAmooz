using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class RejectedModel : UserContextPageModel
    {

        public RejectedModel(IAuthHelper authHelper):base(authHelper)
        {
        }

        public IActionResult OnGet()
        {


            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Confirmed && CurrentAccountRole == Roles.Student)
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
