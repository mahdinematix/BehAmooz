using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Application.Contract.Role;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace ServiceHost.Areas.Administration.Pages.Accounts.Account
{
    public class CreateModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IRoleApplication _roleApplication;
        private readonly IAuthHelper _authHelper;
        public RegisterAccount Command;
        public SelectList Roles;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public CreateModel(IAccountApplication accountApplication, IRoleApplication roleApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _roleApplication = roleApplication;
            _authHelper = authHelper;
        }

        [TempData]
        public string Message { get; set; }
        public IActionResult OnGet()
        {
            var status = _authHelper.CurrentAccountStatus();

            if (status == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (status == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Roles = new SelectList(_roleApplication.GetAllRoles(), "Id", "Name");
            UniTypes = GetUniTypes();
            Unis = GetUnis();

            return Page();
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
