using _01_Framework.Application;
using _01_Framework.Domain;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Domain.WalletAgg;
public class Wallet : EntityBase
{
    public long AccountId { get; private set; }
    public long Balance { get; private set; }
    public List<WalletTransaction> Transactions { get; private set; }
    public Account Account { get; private set; }

    public Wallet(long accountId)
    {
        AccountId = accountId;
        Balance = 0;
        Transactions = new List<WalletTransaction>();
    }

    public void Charge(long amount, long accountId)
    {
        Balance += amount;
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.Deposit, TransactionStatuses.Paid);
        Transactions.Add(transaction);
    }

    public bool BuyFromWallet(long amount, long accountId)
    {
        if (Balance < amount)
            return false;

        Balance -= amount;
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.PayFromWallet, TransactionStatuses.Paid);
        Transactions.Add(transaction);
        return true;
    }
    public bool BuyFromGateway(long amount, long accountId)
    {
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.PayFromGateway, TransactionStatuses.Paid);
        Transactions.Add(transaction);
        return true;
    }

    public bool RequestWithdraw(long amount, string creditCardNo, long accountId)
    {
        if (Balance < amount)
            return false;

        Balance -= amount;
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.Withdraw, TransactionStatuses.Pending, creditCardNo);
        Transactions.Add(transaction);
        return true;
    }

    public void BackMoney(long amount)
    {
        Balance += amount;
    }
}

