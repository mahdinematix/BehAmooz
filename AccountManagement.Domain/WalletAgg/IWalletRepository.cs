using _01_Framework.Domain;
using AccountManagement.Application.Contract.Wallet;

namespace AccountManagement.Domain.WalletAgg;

public interface IWalletRepository : IRepositoryBase<long,Wallet>
{
    Wallet GetByAccountId(long accountId);
    long GetBalanceByAccountId(long accountId);
    List<SettlementRequestViewModel> Search(SettlementRequestSearchModel searchModel);
    List<LogViewModel> GetLogsByAccountId(TransactionLogSearchModel searchModel, long accountId);
    WalletTransaction GetTransactionByTransactionId(long transactionId);
    EditDescription GetDetailsByTransactionId(long transactionId);
}

