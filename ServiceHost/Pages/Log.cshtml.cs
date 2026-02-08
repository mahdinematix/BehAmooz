using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class LogModel : PageModel
    {
        private readonly IAuthHelper _authHelper;
        private readonly IWalletApplication _walletApplication;
        private readonly IOrderApplication _orderApplication;
        public List<LogViewModel> Logs;
        public TransactionLogSearchModel SearchModel;
        [TempData] public string Message { get; set; }

        public LogModel(IAuthHelper authHelper, IWalletApplication walletApplication, IOrderApplication orderApplication)
        {
            _authHelper = authHelper;
            _walletApplication = walletApplication;
            _orderApplication = orderApplication;
        }

        public IActionResult OnGet(TransactionLogSearchModel searchModel)
        {
            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }
            if (_authHelper.CurrentAccountRole() != Roles.Student)
            {
                return RedirectToPage("/Financial/Log" , new {area = "Administration"});
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
        public IActionResult OnGetItems(long id)
        {
            var items = _orderApplication.GetItems(id);
            return Partial("Items", items);
        }



    }
}
