using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class CreateModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;
        public RegisterAccount Command;
        public SelectList Roles;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public CreateModel(IAccountApplication accountApplication, IRoleApplication roleApplication)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
        }

        [TempData]
        public string Message { get; set; }
        public void OnGet()
        {
            Roles = new SelectList(_roleApplication.GetAllRoles(), "Id", "Name");
            UniTypes = GetUniTypes();
            Unis = GetUnis();
        }

        public IActionResult OnPost(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }

            Message = result.Result.Message;
            return RedirectToPage("./Create");

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
                Text = "œ«‰‘ê«Â —« «‰ Œ«» ò‰?œ"
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
                Text = "‰Ê⁄ œ«‰‘ê«Â —« «‰ Œ«» ò‰?œ"
            };

            lstCountries.Insert(0, defItem);

            return lstCountries;
        }
    }
}
