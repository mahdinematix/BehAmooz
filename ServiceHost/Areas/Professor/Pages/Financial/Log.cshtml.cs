using _01_Framework.Application;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Professor.Pages.Financial
{
    public class LogModel : UserContextPageModel
    {
        private readonly IWalletApplication _walletApplication;
        public List<LogViewModel> Logs;
        public TransactionLogSearchModel SearchModel;
        [TempData] public string Message { get; set; }

        public LogModel(IAuthHelper authHelper, IWalletApplication walletApplication):base(authHelper)
        {
            _walletApplication = walletApplication;
        }

        public IActionResult OnGet(TransactionLogSearchModel searchModel)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Logs = _walletApplication.GetLogsByAccountId(searchModel, CurrentAccountId);

            return Page();
        }

        public IActionResult OnGetCancel(CancelRequestWithdrawByUserDto command)
        {
            command.AccountId = CurrentAccountId;
            var result = _walletApplication.CancelRequestWithdrawByUser(command);
            if (!result.IsSucceeded)
            {
                Message = result.Message;
            }
            return RedirectToPage("./Log");
        }
    }
}
