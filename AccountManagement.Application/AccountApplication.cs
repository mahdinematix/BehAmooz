using System.Security.Cryptography.X509Certificates;
using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Domain.AccountAgg;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Domain.WalletAgg;

namespace AccountManagement.Application
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthHelper _authHelper;
        private readonly IFileManager _FileManager;
        private readonly IWalletRepository _walletRepository;

        public AccountApplication(IAccountRepository accountRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IAuthHelper authHelper, IFileManager fileManager, IWalletRepository walletRepository)
        {
            _accountRepository = accountRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _authHelper = authHelper;
            _FileManager = fileManager;
            _walletRepository = walletRepository;
        }

        public async Task<OperationResult> Register(RegisterAccount command)
        {
            var operation = new OperationResult();
            if (_accountRepository.Exists(x => x.NationalCode == command.NationalCode || x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecordNationalCodeOrCode);
            }

            if (command.NationalCode.Length != 10 || string.IsNullOrWhiteSpace(command.NationalCode))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }
            int[] numArray = command.NationalCode.Select(c => (int)char.GetNumericValue(c)).ToArray();
            int num2 = numArray[9];
            string[] invalidCodes = new string[]
            {
                "0000000000", "1111111111", "2222222222", "3333333333",
                "4444444444", "5555555555", "6666666666", "7777777777",
                "8888888888", "9999999999"
            };
            if (invalidCodes.Contains(command.NationalCode))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }

            int num3 = 0;

            for (int i = 0; i < 9; i++)
            {
                num3 += numArray[i] * (10 - i);
            }

            int num4 = num3 % 11;

            if (!((num4 == 0 && num2 == 0) || (num4 == 1 && num2 == 1) || (num4 > 1 && num2 == 11 - num4)))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }

            if (command.Password != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }
            var password = _passwordHasher.Hash(command.Password);
            var fileUrlForNationalCardPicture = await _FileManager.Upload(command.NationalCardPicture, false);
            var account = new Account(command.FirstName, command.LastName, password, command.Email, command.PhoneNumber,
                command.NationalCode, command.Code, command.UniversityType, command.University, command.Major,
                fileUrlForNationalCardPicture, command.RoleId, command.EducationLevel);
            _accountRepository.Create(account);
            _accountRepository.Save();

            var wallet = new Wallet(account.Id);
            _walletRepository.Create(wallet);
            _walletRepository.Save();
            return operation.Succeed();
        }

        public async Task<OperationResult> Edit(EditAccount command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_accountRepository.Exists(x =>x.NationalCode == command.NationalCode && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecordNationalCodeOrCode);
            }

            if (_accountRepository.Exists(x => x.Code == command.Code && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecordNationalCodeOrCode);
            }

            if (command.NationalCode.Length != 10 || string.IsNullOrWhiteSpace(command.NationalCode))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }
            int[] numArray = command.NationalCode.Select(c => (int)char.GetNumericValue(c)).ToArray();
            int num2 = numArray[9];
            string[] invalidCodes = new string[]
            {
                "0000000000", "1111111111", "2222222222", "3333333333",
                "4444444444", "5555555555", "6666666666", "7777777777",
                "8888888888", "9999999999"
            };
            if (invalidCodes.Contains(command.NationalCode))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }

            int num3 = 0;

            for (int i = 0; i < 9; i++)
            {
                num3 += numArray[i] * (10 - i);
            }

            int num4 = num3 % 11;

            if (!((num4 == 0 && num2 == 0) || (num4 == 1 && num2 == 1) || (num4 > 1 && num2 == 11 - num4)))
            {
                return operation.Failed(ApplicationMessages.InvalidNationalCode);
            }
            var fileUrlForNationalCardPicture = await _FileManager.Upload(command.NationalCardPicture, false);
            account.Edit(command.FirstName, command.LastName, command.Email, command.PhoneNumber,
                command.NationalCode, command.Code, command.UniversityType, command.University, command.Major,
                fileUrlForNationalCardPicture, command.RoleId, command.EducationLevel);
            if (account.RoleId != 1)
            {
                if (_authHelper.CurrentAccountRole() == Roles.Professor)
                {
                    account.ChangeStatusToWaiting();
                }
            }
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

            if (command.NewPassword != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }

            var password = _passwordHasher.Hash(command.NewPassword);
            account.ChangePassword(password);
            _accountRepository.Save();
            return operation.Succeed();
        }

        public OperationResult ChangePasswordByUser(ChangePassword command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetBy(command.Id);
            if (account == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            var result = _passwordHasher.Check(account.Password, command.Password);

            if (!result.Verified)
            {
                return operation.Failed(ApplicationMessages.WrongNowPassword);
            }

            if (command.NewPassword != command.RePassword)
            {
                return operation.Failed(ApplicationMessages.PasswordsNotMatch);
            }

            var password = _passwordHasher.Hash(command.NewPassword);
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
            var authViewModel = new AuthViewModel(account.Id, account.RoleId, fullname, account.NationalCode, account.Code, account.UniversityTypeId, account.UniversityId, account.MajorId, account.NationalCardPicture, account.Status, account.PhoneNumber, account.Email, permissions, command.Password,account.EducationLevel);

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

        public List<AccountViewModel> SearchInStudents(AccountSearchModel searchModel)
        {
            return _accountRepository.SearchInStudents(searchModel);
        }

        public List<AccountViewModel> SearchInCustomers(AccountSearchModel searchModel)
        {
            return _accountRepository.SearchInCustomers(searchModel);
        }
    }
}
