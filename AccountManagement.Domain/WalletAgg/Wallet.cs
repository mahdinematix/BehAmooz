using _01_Framework.Application;
using _01_Framework.Domain;
using AccountManagement.Domain.AccountAgg;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

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
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.Deposit, TransactionStatuses.Paid,0);
        Transactions.Add(transaction);
    }

    public bool BuyFromWallet(long amount, long accountId, long orderId)
    {
        if (Balance < amount)
            return false;

        Balance -= amount;
        var transaction = new WalletTransaction(
            accountId,
            amount,
            TransactionTypes.PayFromWallet,
            TransactionStatuses.Paid,
            orderId);
        Transactions.Add(transaction);
        return true;
    }
    public bool BuyFromGateway(long amount, long accountId, long orderId)
    {
        var transaction = new WalletTransaction(
            accountId,
            amount,
            TransactionTypes.PayFromGateway,
            TransactionStatuses.Paid,
            orderId);
        Transactions.Add(transaction);
        return true;
    }

    public bool RequestWithdraw(long amount, string creditCardNo, long accountId)
    {
        if (Balance < amount)
            return false;

        Balance -= amount;
        var transaction = new WalletTransaction(accountId, amount, TransactionTypes.Withdraw, TransactionStatuses.Pending, 0,creditCardNo);
        Transactions.Add(transaction);
        return true;
    }

    public void BackMoney(long amount)
    {
        Balance += amount;
    }
}

