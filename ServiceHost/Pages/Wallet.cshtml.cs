using _01_Framework.Application;
using _01_Framework.Application.ZarinPal;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class WalletModel : UserContextPageModel
    {
        private readonly IWalletApplication _walletApplication;
        private readonly IZarinPalFactory _zarinPalFactory;
        public long Balance;
        public ChargeWalletDto Command;
        [TempData] public string Message { get; set; }

        public WalletModel(IAuthHelper authHelper, IWalletApplication walletApplication, IZarinPalFactory zarinPalFactory) : base(authHelper)
        {
            _walletApplication = walletApplication;
            _zarinPalFactory = zarinPalFactory;
        }

        public IActionResult OnGet()
        {

            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }

            if (CurrentAccountRole == Roles.Professor)
            {
                return RedirectToPage("/Financial/Wallet", new { area = "Administration" });
            }

            if (CurrentAccountRole == Roles.Administrator || CurrentAccountRole == Roles.SuperAdministrator)
            {
                return RedirectToPage("/Index");
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
            var paymentData = _zarinPalFactory.CreatePaymentRequest(
                command.Amount.ToString(), CurrentAccountInfo.Mobile, CurrentAccountInfo.Email,
                $"ChargeWallet: {command.Amount}", 0, PaymentTypes.ChargeWallet);

            return Redirect(
                $"https://{_zarinPalFactory.Prefix}.zarinpal.com/pg/StartPay/{paymentData.authority}");
            
        }

        public IActionResult OnGetCallBack([FromQuery] string authority, [FromQuery] string status, [FromQuery] long amount)
        {


            var verificationResponse = _zarinPalFactory.CreateVerificationRequest(authority, amount.ToString());

            if (status == "OK" && verificationResponse.code >= 100)
            {
                var command = new ChargeWalletDto
                {
                    AccountId = CurrentAccountId,
                    Amount = amount
                };
                var result = _walletApplication.ChargeWallet(command);
                Message = result.Message;
                return RedirectToPage();
            }
            var res = new PaymentResult().Failed(ApplicationMessages.PaymentFailed);
            return RedirectToPage("/PaymentResult", res);
        }
    }
}
