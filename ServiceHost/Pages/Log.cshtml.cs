using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StudyManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class LogModel : UserContextPageModel
    {
        private readonly IWalletApplication _walletApplication;
        private readonly IOrderApplication _orderApplication;
        public List<LogViewModel> Logs;
        public TransactionLogSearchModel SearchModel;
        [TempData] public string Message { get; set; }

        public LogModel(IAuthHelper authHelper, IWalletApplication walletApplication, IOrderApplication orderApplication):base(authHelper)
        {
            _walletApplication = walletApplication;
            _orderApplication = orderApplication;
        }

        public IActionResult OnGet(TransactionLogSearchModel searchModel)
        {
            if (!IsAuthenticated)
            {
                return RedirectToPage("/Login");
            }
            if (CurrentAccountRole != Roles.Student)
            {
                return RedirectToPage("/Financial/Log" , new {area = "Administration"});
            }

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
        public IActionResult OnGetItems(long id)
        {
            var items = _orderApplication.GetItems(id);
            return Partial("Items", items);
        }



    }
}
