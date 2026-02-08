using _01_Framework.Application;
using _01_Framework.Domain;

namespace AccountManagement.Domain.WalletAgg;

public class WalletTransaction : EntityBase
{
    public long AccountId { get; private set; }
    public long Amount { get; private set; }
    public int Type { get; private set; }
    public int Status { get; private set; }
    public string CreditCardNo { get; set; }
    public string Description { get; private set; }
    public long OrderId { get; private set; }
    public long WalletId { get; private set; }
    public Wallet Wallet { get; private set; }

    public WalletTransaction(long accountId, long amount, int type, int status, long orderId, string creditCardNo= "")
    {
        AccountId = accountId;
        Amount = amount;
        Type = type;
        Status = status;
        CreditCardNo = creditCardNo;
        Description = "";
        OrderId = orderId;
    }

    public void Pay(string description)
    {
        Status = TransactionStatuses.Paid;
        Description = description;
    }

    public void Reject(string description)
    {
        Status = TransactionStatuses.Rejected;
        Description = description;
    }

    public void Cancel(string description="لغو شده توسط کاربر")
    {
        Status = TransactionStatuses.Canceled;
        Description = description;
    }

    public void EditDescription(string description)
    {
        Description = description;
    }
}


