using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Financial
{
    public class LogModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly IWalletApplication _walletApplication;
        public List<LogViewModel> Logs;
        public LogSearchModel SearchModel;
        [TempData] public string Message { get; set; }

        public LogModel(IAuthHelper authHelper, IWalletApplication walletApplication)
        {
            _authHelper = authHelper;
            _walletApplication = walletApplication;
        }

        public IActionResult OnGet(LogSearchModel searchModel)
        {
            if (_authHelper.CurrentAccountRole() != Roles.Professor)
            {
                return RedirectToPage("/Index");
            }
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            Logs = _walletApplication.GetLogsByAccountId(searchModel, _authHelper.CurrentAccountId());


            return Page();
        }

        public IActionResult OnGetCancel(CancelRequestWithdrawByUserDto command)
        {
            command.AccountId = _authHelper.CurrentAccountId();
            var result = _walletApplication.CancelRequestWithdrawByUser(command);
            if (!result.IsSucceeded)
            {
                Message = result.Message;
            }
            return RedirectToPage("./Log");
        }
    }
}
