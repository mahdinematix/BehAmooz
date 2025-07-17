using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;

        public RegisterModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        [TempData] public string RegisterMessage { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPost(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("/Login");
            }
            RegisterMessage = result.Message;
            return RedirectToPage();
        }
    }
}
