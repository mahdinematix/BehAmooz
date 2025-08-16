﻿using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using AccountManagement.Infrastructure.Configuration.Permission;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public List<AccountViewModel> Students;
        public SelectList Roles;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }
        [NeedsPermissions(AccountPermissions.ListAccounts)]
        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new SelectList(_roleApplication.GetAllRoles(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
            Students = _accountApplication.SearchInStudents(searchModel);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
        }

        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword {Id = id};
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

        private List<SelectListItem> GetUnis(int typeId = 1)
        {

            List<SelectListItem> lstUnis = Universities.List
                .Where(c => c.UniversityTypeId == typeId)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = "دانشگاه را انتخاب کنید"
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
                Text = "نوع دانشگاه را انتخاب کنید"
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }

    }
}
