using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;

        public LoginModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        [TempData] public string LoginMessage { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(Login command)
        {
            var result = _accountApplication.Login(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("/Index");
            }
            LoginMessage = result.Message;
            return RedirectToPage("/Login");
        }

        public IActionResult OnGetLogout()
        {
            _accountApplication.Logout();
            return RedirectToPage("/Login");
        }
    }
}
