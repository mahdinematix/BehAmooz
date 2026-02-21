using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class WalletModel : UserContextPageModel
    {
        private readonly IWalletApplication _walletApplication;
        public long Balance;
        public ChargeWalletDto Command;
        [TempData] public string Message { get; set; }

        public WalletModel(IAuthHelper authHelper, IWalletApplication walletApplication):base(authHelper)
        {
            _walletApplication = walletApplication;
        }

        public IActionResult OnGet()
        {

            if (IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Financial/Wallet", new { area = "Administration" });
            }

            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Balance = _walletApplication.GetBalanceByAccountId(CurrentAccountId);

            return Page();
        }

        public IActionResult OnPost(ChargeWalletDto command)
        {
            command.AccountId = CurrentAccountId;
            var result = _walletApplication.ChargeWallet(command);
            Message = result.Message;
            return RedirectToPage();
        }
    }
}
