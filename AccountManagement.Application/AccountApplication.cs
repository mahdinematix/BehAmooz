using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
        }

        public OperationResult Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.NationalCode == command.NationalCode || x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecordNationalCodeOrCode);
            }
            if (command.Password != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }
            var password = _passwordHasher.Hash(command.Password);
            var account = new Account(command.FirstName, command.LastName, password, command.Email, command.PhoneNumber,
                command.NationalCode, command.Code, command.UniversityType, command.University, command.Major,
                command.NationalCardPicture,command.RoleId);
            _accountRepository.Create(account);
            _accountRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_accountRepository.Exists(x=>(x.NationalCode == command.NationalCode || x.Code == command.Code) && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecordNationalCodeOrCode);
            }

            account.Edit(command.FirstName, command.LastName, command.Email, command.PhoneNumber,
                command.NationalCode, command.Code, command.UniversityType, command.University, command.Major,
                command.NationalCardPicture,command.RoleId);
            _accountRepository.Save();
            return operation.Succeed();

        }

        public OperationResult Confirm(long id)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            account.Confirm();
            _accountRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Reject(long id)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            account.Reject();
            _accountRepository.Save();
            return operation.Succeed();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (command.Password != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }

            var password = _passwordHasher.Hash(command.Password);
            account.ChangePassword(password);
            _accountRepository.Save();
            return operation.Succeed();
        }

        public EditAccount GetDetails(long id)
        {
            return _accountRepository.GetDetails(id);
        }

        public List<AccountViewModel> Search(AccountSearchModel searchModel)
        {
            return _accountRepository.Search(searchModel);
        }
    }
}
