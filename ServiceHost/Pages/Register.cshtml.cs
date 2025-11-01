using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public RegisterAccount Command;
        [TempData] public string RegisterMessage { get; set; }

        public RegisterModel(IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {

            if (_authHelper.IsAuthenticated())
            {
                var status = _authHelper.CurrentAccountStatus();
                if (_authHelper.CurrentAccountRole() == Roles.Student)
                {
                    return RedirectToPage("/Index");
                }
                if (_authHelper.CurrentAccountRole() != Roles.Student)
                {
                    return RedirectToPage("/Index", new { area = "Administration" });
                }

                if (status == Statuses.Waiting)
                {
                    return RedirectToPage("/NotConfirmed");
                }

                if (status == Statuses.Rejected)
                {
                    return RedirectToPage("/Reject");
                }

            }
            

            Command = new RegisterAccount
            {
                RoleId = 3
            };
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnPost(RegisterAccount command)
        {
            var result = _accountApplication.Register(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("/Login");
            }
            RegisterMessage = result.Result.Message;
            return RedirectToPage();
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
