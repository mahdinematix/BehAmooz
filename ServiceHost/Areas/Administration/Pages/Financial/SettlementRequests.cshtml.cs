using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Financial
{
    public class SettlementRequestsModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        public List<SettlementRequestViewModel> SettlementRequests;
        public SettlementRequestSearchModel SearchModel;
        private readonly IWalletApplication _walletApplication;
        [TempData] public string Message { get; set; }

        public SettlementRequestsModel(IAuthHelper authHelper, IWalletApplication walletApplication)
        {
            _authHelper = authHelper;
            _walletApplication = walletApplication;
        }

        public IActionResult OnGet(SettlementRequestSearchModel searchModel)
        {
            if (_authHelper.CurrentAccountRole() != Roles.Administrator)
            {
                return RedirectToPage("/Index");
            }

            SettlementRequests = _walletApplication.Search(searchModel);

            return Page();
        }

        public IActionResult OnGetPay(long transactionId)
        {
            var command = new PayRequestWithdrawDto
            {
                AccountId = _authHelper.CurrentAccountId(),
                TransactionId = transactionId
            };
            return Partial("TrackingCode", command);
        }

        public IActionResult OnPostPay(PayRequestWithdrawDto command)
        {
            var result = _walletApplication.PayRequestWithdraw(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetReject(long transactionId)
        {
            var command = new RejectRequestWithdrawDto
            {
                AccountId = _authHelper.CurrentAccountId(),
                TransactionId = transactionId
            };
            return Partial("Description", command);
        }

        public IActionResult OnPostReject(RejectRequestWithdrawDto command)
        {
            var result = _walletApplication.RejectRequestWithdraw(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long transactionId)
        {
            var command = _walletApplication.GetDetailsByTransactionId(transactionId);
            return Partial("EditDescription", command);
        }

        public IActionResult OnPostEdit(EditDescription command)
        {
            var result = _walletApplication.EditDescription(command);
            return new JsonResult(result);
        }
    }
}
