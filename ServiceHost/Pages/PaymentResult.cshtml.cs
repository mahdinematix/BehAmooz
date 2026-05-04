using _01_Framework.Application;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class PaymentResultModel : UserContextPageModel
    {
        public PaymentResult Result;

        public PaymentResultModel(IAuthHelper authHelper):base(authHelper)
        {
        }
        public IActionResult OnGet(PaymentResult result)
        {
            if (!IsAuthenticated)
                return RedirectToPage("/Login");

            if (CurrentAccountRole == Roles.Professor)
                return RedirectToPage("/Index", new { area = "Administration" });

            if (CurrentAccountStatus == Statuses.Waiting)
                return RedirectToPage("/NotConfirmed");

            if (CurrentAccountStatus == Statuses.Rejected)
                return RedirectToPage("/Reject");
            Result = result;

            return Page();
        }
    }
}
