using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Application.Contract.Wallet;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;


namespace ServiceHost.Areas.Administration.Pages.Account
{
    public class IndexModel : UserContextPageModel
    {
        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public SelectList RolesList;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public string UniversityName { get; set; }
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;
        private readonly IWalletApplication _walletApplication;
        private readonly IUniversityApplication _universityApplication;
        public bool ShowUniversityFiled { get; set; }

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication, IAuthHelper authHelper, IWalletApplication walletApplication, IUniversityApplication universityApplication) : base(authHelper)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
            _walletApplication = walletApplication;
            _universityApplication = universityApplication;
        }

        [NeedsPermissions(AccountPermissions.ListAccounts)]
        public IActionResult OnGet(AccountSearchModel searchModel)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }

            ShowUniversityFiled = CurrentAccountRole == Roles.SuperAdministrator;
            
            RolesList = new SelectList(_roleApplication.GetAllRoles(CurrentAccountRole), "Id", "Name");

            UniversityName = _universityApplication.GetNameBy(CurrentAccountUniversityId);

            Accounts = _accountApplication.Search(searchModel,CurrentAccountRole, CurrentAccountUniversityId, CurrentAccountInfo.UniversityTypeId);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword { Id = id };
            return Partial("ChangePassword", command);
        }

        [NeedsPermissions(AccountPermissions.ChangePassword)]
        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }

        [NeedsPermissions(AccountPermissions.ConfirmOrReject)]
        public IActionResult OnGetConfirm(long id)
        {
            _accountApplication.Confirm(id);
            return RedirectToPage("./Index");
        }

        [NeedsPermissions(AccountPermissions.ConfirmOrReject)]
        public IActionResult OnGetReject(long id)
        {
            _accountApplication.Reject(id);
            return RedirectToPage("./Index");
        }


        [NeedsPermissions(AccountPermissions.Logs)]
        public IActionResult OnGetLog(long id)
        {
            var searchModel = new TransactionLogSearchModel
            {
                Status = 0,
                Type = 0
            };
            var logs = _walletApplication.GetLogsByAccountId(searchModel, id);
            return Partial("Log", logs);
        }

        private List<SelectListItem> GetUnis(int typeId = 0)
        {

            List<SelectListItem> lstUnis = _universityApplication.GetUniversitiesByType(typeId).Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversity
            };

            lstUnis.Insert(0, defItem);

            return lstUnis;
        }

        private List<SelectListItem> GetUniTypes()
        {
            var lstCountries = new List<SelectListItem>();

            List<UniversityTypeViewModel> Countries = UniversityTypes.List;

            lstCountries = Countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = ApplicationMessages.SelectYourUniversityType
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }

    }
}
