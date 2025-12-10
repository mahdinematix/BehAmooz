using _01_Framework.Application;

namespace AccountManagement.Application.Contract.Wallet
{
    public interface IWalletApplication
    {
        OperationResult BuyFromWallet(BuyFromWalletDto command);
        OperationResult BuyFromGateway(BuyFromWalletDto command);
        OperationResult ChargeWallet(ChargeWalletDto command);
        OperationResult RequestWithdraw(RequestWithdrawDto command);
        OperationResult PayRequestWithdraw(PayRequestWithdrawDto command);
        OperationResult RejectRequestWithdraw(RejectRequestWithdrawDto command);
        OperationResult CancelRequestWithdrawByUser(CancelRequestWithdrawByUserDto command);
        long GetBalanceByAccountId(long accountId);
        List<SettlementRequestViewModel> Search(SettlementRequestSearchModel searchModel);
        List<LogViewModel> GetLogsByAccountId(LogSearchModel searchModel, long accountId);
        OperationResult EditDescription(EditDescription command);
        EditDescription GetDetailsByTransactionId(long transactionId);
        void PayToProfessor(int sessionPrice, long professorId);
    }
}
