using _01_Framework.Application;
using AccountManagement.Application.Contract.Wallet;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Professor.Pages.Financial;

public class WalletModel : UserContextPageModel
{
    private readonly IWalletApplication _walletApplication;
    public long Balance;
    public RequestWithdrawDto Command;
    [TempData] public string Message { get; set; }

    public WalletModel(IAuthHelper authHelper, IWalletApplication walletApplicationl):base(authHelper)
    {
        _walletApplication = walletApplicationl;
    }

    public IActionResult OnGet()
    {
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

    public IActionResult OnPost(RequestWithdrawDto command)
    {
        command.AccountId = CurrentAccountId;
        var result = _walletApplication.RequestWithdraw(command);
        Message = result.Message;
        return RedirectToPage();
    }
}

