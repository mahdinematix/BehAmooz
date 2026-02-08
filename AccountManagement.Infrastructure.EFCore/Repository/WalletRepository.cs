using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Wallet;
using AccountManagement.Domain.WalletAgg;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class WalletRepository : RepositoryBase<long , Wallet>, IWalletRepository
    {
        private readonly AccountContext _context;
        public WalletRepository(AccountContext context) : base(context) 
        {
            _context = context;
        }

        public Wallet GetByAccountId(long accountId)
        {
            return _context.Wallets.FirstOrDefault(x => x.AccountId == accountId); 
        }

        public long GetBalanceByAccountId(long accountId)
        {
            return _context.Wallets.FirstOrDefault(x => x.AccountId == accountId).Balance;
        }

        public List<SettlementRequestViewModel> Search(SettlementRequestSearchModel searchModel)
        {
            var query = _context.Wallets
                .Include(w => w.Account)
                .SelectMany(w => w.Transactions.Where(t => t.Type == TransactionTypes.Withdraw),
                    (w, t) => new { Wallet = w, Transaction = t });

            if (!string.IsNullOrWhiteSpace(searchModel.FullName))
            {
                query = query.Where(x => x.Wallet.Account.FirstName.Contains(searchModel.FullName)
                                         || x.Wallet.Account.LastName.Contains(searchModel.FullName));
            }

            if (searchModel.RoleId > 0)
                query = query.Where(x => x.Wallet.Account.RoleId == searchModel.RoleId);

            if (!string.IsNullOrWhiteSpace(searchModel.NationalCode))
                query = query.Where(x => x.Wallet.Account.NationalCode.Contains(searchModel.NationalCode));

            if (searchModel.Status > 0)
                query = query.Where(x => x.Transaction.Status == searchModel.Status);


            var result = query.Select(x => new SettlementRequestViewModel
            {
                TransactionId = x.Transaction.Id,
                FullName = x.Wallet.Account.FirstName + " " + x.Wallet.Account.LastName,
                NationalCode = x.Wallet.Account.NationalCode,
                RoleId = x.Wallet.Account.RoleId,
                CreditCardNo = x.Transaction.CreditCardNo,
                Amount = x.Transaction.Amount,
                Status = x.Transaction.Status,
                CreationDate = x.Transaction.CreationDate.ToFarsi(),
                Description = x.Transaction.Description
            }).OrderByDescending(x => x.TransactionId).ToList();

            return result;

        }

        public List<LogViewModel> GetLogsByAccountId(TransactionLogSearchModel searchModel, long accountId)
        {
            var query = _context.Wallets
                .Include(w => w.Account).Where(x=>x.AccountId == accountId)
                .SelectMany(w => w.Transactions,
                    (w, t) => new { Wallet = w, Transaction = t });


            if (searchModel.Type > 0)
                query = query.Where(x => x.Transaction.Type == searchModel.Type);

            if (searchModel.Status > 0)
                query = query.Where(x => x.Transaction.Status == searchModel.Status);

            var result = query.Select(x => new LogViewModel
            {
                TransactionId = x.Transaction.Id,
                FullName = x.Wallet.Account.FirstName + " " + x.Wallet.Account.LastName,
                NationalCode = x.Wallet.Account.NationalCode,
                RoleId = x.Wallet.Account.RoleId,
                CreditCardNo = x.Transaction.CreditCardNo,
                Amount = x.Transaction.Amount,
                Status = x.Transaction.Status,
                CreationDate = x.Transaction.CreationDate.ToFarsi(),
                Description = x.Transaction.Description,
                Type = x.Transaction.Type, 
                OrderId = x.Transaction.OrderId
            }).OrderByDescending(x => x.TransactionId).ToList();

            return result;
        }

        public WalletTransaction GetTransactionByTransactionId(long transactionId)
        {
            return _context.WalletTransactions.FirstOrDefault(x => x.Id == transactionId);
        }

        public EditDescription GetDetailsByTransactionId(long transactionId)
        {
            return _context.WalletTransactions.Select(x => new EditDescription
            {
                AccountId = x.AccountId,
                Description = x.Description,
                TransactionId = x.Id
            }).FirstOrDefault(x => x.TransactionId == transactionId);
        }

    }
}
