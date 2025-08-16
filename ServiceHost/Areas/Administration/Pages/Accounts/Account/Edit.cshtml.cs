using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class EditModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;
        public EditAccount Command;
        public SelectList Roles;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        [TempData] public string Message { get; set; }

        public EditModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        public void OnGet(long id)
        {
            Command = _accountApplication.GetDetails(id);
            Roles = new SelectList(_roleApplication.GetAllRoles(), "Id", "Name");
            UniTypes = GetUniTypes();
            Unis = GetUnis();
        }

        public IActionResult OnPost(EditAccount command)
        {

            var result = _accountApplication.Edit(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Edit");
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
