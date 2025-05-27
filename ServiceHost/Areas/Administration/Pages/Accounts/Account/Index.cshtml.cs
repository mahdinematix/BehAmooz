using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string Message { get; set; }
        public AccountSearchModel SearchModel;
        public List<AccountViewModel> Accounts;
        public SelectList Roles;
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;

        public IndexModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        public void OnGet(AccountSearchModel searchModel)
        {
            Roles = new SelectList(_roleApplication.GetAllRoles(), "Id", "Name");
            Accounts = _accountApplication.Search(searchModel);
        }

        public IActionResult OnGetCreate()
        {
            var command = new RegisterAccount
            {
                Roles = _roleApplication.GetAllRoles()
            };
            return Partial("./Create", command);
        }

        public JsonResult OnPostCreate(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var account = _accountApplication.GetDetails(id);
            account.Roles = _roleApplication.GetAllRoles();
            return Partial("Edit", account);
        }

        public JsonResult OnPostEdit(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword {Id = id};
            return Partial("ChangePassword", command);
        }


        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePassword(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetConfirm(long id)
        {
            _accountApplication.Confirm(id);
            return RedirectToPage("./Index");
        }
        public IActionResult OnGetReject(long id)
        {
            _accountApplication.Reject(id);
            return RedirectToPage("./Index");

        }
    }
}
