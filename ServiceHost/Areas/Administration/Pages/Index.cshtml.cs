using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Administration.Pages
{
    public class IndexModel : UserContextPageModel
    {
        public IndexModel(IAuthHelper authHelper): base(authHelper)
        {
        }

        public IActionResult OnGet()
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            return Page();
        }
    }
}
