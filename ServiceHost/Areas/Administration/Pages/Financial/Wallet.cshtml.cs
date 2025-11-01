using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Administration.Pages.Financial;

public class WalletModel : PageModel
{
    private readonly IAuthHelper _authHelper;
    private readonly IWalletApplication _walletApplication;
    public long Balance;
    public RequestWithdrawDto Command;
    [TempData] public string Message { get; set; }

    public WalletModel(IAuthHelper authHelper, IWalletApplication walletApplicationl)
    {
        _authHelper = authHelper;
        _walletApplication = walletApplicationl;
    }

    public IActionResult OnGet()
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

        Balance = _walletApplication.GetBalanceByAccountId(_authHelper.CurrentAccountId());

        return Page();
    }

    public IActionResult OnPost(RequestWithdrawDto command)
    {
        command.AccountId = _authHelper.CurrentAccountId();
        var result = _walletApplication.RequestWithdraw(command);
        Message = result.Message;
        return RedirectToPage();
    }
}

