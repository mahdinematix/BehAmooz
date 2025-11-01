using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class WalletModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly IWalletApplication _walletApplication;
        public long Balance;
        public ChargeWalletDto Command;
        [TempData] public string Message { get; set; }

        public WalletModel(IAuthHelper authHelper, IWalletApplication walletApplication)
        {
            _authHelper = authHelper;
            _walletApplication = walletApplication;
        }

        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();

            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Financial/Wallet", new { area = "Administration" });
            }

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Balance = _walletApplication.GetBalanceByAccountId(_authHelper.CurrentAccountId());

            return Page();
        }

        public IActionResult OnPost(ChargeWalletDto command)
        {
            command.AccountId = _authHelper.CurrentAccountId();
            var result = _walletApplication.ChargeWallet(command);
            Message = result.Message;
            return RedirectToPage();
        }
    }
}
