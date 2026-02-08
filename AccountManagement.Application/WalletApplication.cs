using _01_Framework.Application;
using AccountManagement.Application.Contract.Wallet;
using AccountManagement.Domain.WalletAgg;

namespace AccountManagement.Application
{
    public class WalletApplication : IWalletApplication
    {
        private readonly IWalletRepository _walletRepository;

        public WalletApplication(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public OperationResult BuyFromWallet(BuyFromWalletDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            var result = wallet.BuyFromWallet(command.Amount, command.AccountId, command.OrderId);
            if (!result)
            {
                return operation.Failed(ApplicationMessages.RequestedAmountMoreThanYourBalance);
            }
            _walletRepository.Save();
            return operation.Succeed();
        }

        public OperationResult BuyFromGateway(BuyFromWalletDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            wallet.BuyFromGateway(command.Amount, command.AccountId, command.OrderId);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public OperationResult ChargeWallet(ChargeWalletDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            wallet.Charge(command.Amount,command.AccountId);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public OperationResult RequestWithdraw(RequestWithdrawDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            var result = wallet.RequestWithdraw(command.Amount, command.CreditCardNo, command.AccountId);
            if (!result)
                return operation.Failed(ApplicationMessages.RequestedAmountMoreThanYourBalance);

            _walletRepository.Save();
            return operation.Succeed($"درخواست برداشت شما به مبلغ {command.Amount.ToMoney()} تومان با موفقیت ثبت شد و تا 7 روز کاری نتیجه به شما اعلام گردیده و یا مبلغ فوق به حساب معرفی شده از سوی شما واریز می گردد.");

        }

        public OperationResult PayRequestWithdraw(PayRequestWithdrawDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            var transaction = _walletRepository.GetTransactionByTransactionId(command.TransactionId);
            if (transaction == null || transaction.Type != TransactionTypes.Withdraw ||
                transaction.Status != TransactionStatuses.Pending)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            transaction.Pay(command.Description);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public OperationResult RejectRequestWithdraw(RejectRequestWithdrawDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            var transaction = _walletRepository.GetTransactionByTransactionId(command.TransactionId);
            if (transaction == null || transaction.Type != TransactionTypes.Withdraw ||
                transaction.Status != TransactionStatuses.Pending)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            transaction.Reject(command.Description);
            wallet.BackMoney(transaction.Amount);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public OperationResult CancelRequestWithdrawByUser(CancelRequestWithdrawByUserDto command)
        {
            var operation = new OperationResult();
            var wallet = _walletRepository.GetByAccountId(command.AccountId);
            if (wallet == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            var transaction = _walletRepository.GetTransactionByTransactionId(command.TransactionId);
            if (transaction == null || transaction.Type != TransactionTypes.Withdraw ||
                transaction.Status != TransactionStatuses.Pending)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            transaction.Cancel();
            wallet.BackMoney(transaction.Amount);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public long GetBalanceByAccountId(long accountId)
        {
            return _walletRepository.GetBalanceByAccountId(accountId);
        }

        public List<SettlementRequestViewModel> Search(SettlementRequestSearchModel searchModel)
        {
            return _walletRepository.Search(searchModel);
        }

        public List<LogViewModel> GetLogsByAccountId(TransactionLogSearchModel searchModel, long accountId)
        {
            return _walletRepository.GetLogsByAccountId(searchModel, accountId);
        }

        public OperationResult EditDescription(EditDescription command)
        {
            var operation = new OperationResult();
            var transaction = _walletRepository.GetTransactionByTransactionId(command.TransactionId);
            if (transaction == null || transaction.Type != TransactionTypes.Withdraw ||
                transaction.Status == TransactionStatuses.Pending)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            transaction.EditDescription(command.Description);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public EditDescription GetDetailsByTransactionId(long transactionId)
        {
            return _walletRepository.GetDetailsByTransactionId(transactionId);
        }

        public void PayToProfessor(int sessionPrice, long professorId)
        {
            var wallet = _walletRepository.GetByAccountId(professorId);
            int organShare = (int)(sessionPrice * 0.25);
            int tax = (int)(sessionPrice * 0.10);
            int professorShare = sessionPrice - organShare - tax;
            wallet.Charge(professorShare,professorId);
        }
    }
}
