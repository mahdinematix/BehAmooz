using _01_Framework.Application;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthHelper _authHelper;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IAuthHelper authHelper)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _authHelper = authHelper;
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

        public OperationResult Login(Login command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetByNationalCode(command.NationalCode);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.WrongPasswordOrUsername);
            }

            var result = _passwordHasher.Check(account.Password, command.Password);

            if (!result.Verified)
            {
                return operation.Failed(ApplicationMessages.WrongPasswordOrUsername);
            }

            var permissions = _roleRepository.GetBy(account.RoleId)
                .Permissions
                .Select(x => x.Code)
                .ToList();
            var fullname = account.FirstName + " " + account.LastName;
            var authViewModel = new AuthViewModel(account.Id,account.RoleId,fullname,account.NationalCode,account.Code,account.UniversityTypeId,account.UniversityId,account.MajorId,account.NationalCardPicture,account.Status,account.PhoneNumber,account.Email,permissions);

            _authHelper.Signin(authViewModel);
            return operation.Succeed();
        }

        public void Logout()
        {
            _authHelper.SignOut();
        }

        public List<AccountViewModel> GetProfessors()
        {
            return _accountRepository.GetProfessors();
        }

        public string GetProfessorById(long professorId)
        {
            return _accountRepository.GetProfessorById(professorId);
        }
    }
}
