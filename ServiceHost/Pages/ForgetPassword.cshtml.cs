using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class ForgetPasswordModel : UserContextPageModel
    {
        public string HiddenPhoneNumber { get; set; }
        [TempData] public string Message { get; set; }

        public ForgetPasswordModel(IAuthHelper authHelper) : base(authHelper)
        {
        }

        public IActionResult OnGet()
        {
            if (IsAuthenticated)
            {
                if (CurrentAccountRole == Roles.Student)
                {
                    return RedirectToPage("/Index");
                }
                if (CurrentAccountRole == Roles.Administrator || CurrentAccountRole == Roles.SuperAdministrator)
                {
                    return RedirectToPage("/Index", new { area = "Administration" });
                }
                if (CurrentAccountRole == Roles.Professor)
                {
                    return RedirectToPage("/Index", new { area = "Professor" });
                }

                if (CurrentAccountStatus == Statuses.Waiting)
                {
                    return RedirectToPage("/NotConfirmed");
                }

                if (CurrentAccountStatus == Statuses.Rejected)
                {
                    return RedirectToPage("/Reject");
                }
            }


            return Page();
        }

        public IActionResult OnPost(string nationalCode)
        {
            return RedirectToPage("Sms", new { nationalCode });
        }

    }
}
