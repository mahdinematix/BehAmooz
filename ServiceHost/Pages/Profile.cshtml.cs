using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IAuthHelper _authHelper;
        public EditAccount Command;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        [TempData] public bool IsSucceeded { get; set; }
        [TempData] public string Message { get; set; }

        public ProfileModel(IAccountApplication accountApplication, IAuthHelper authHelper)
        {
            _accountApplication = accountApplication;
            _authHelper = authHelper;
        }

        public IActionResult OnGet()
        {
            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            var id = _authHelper.CurrentAccountId();
            Command = _accountApplication.GetDetails(id);

            return Page();
        }

        public IActionResult OnPost(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            IsSucceeded = result.Result.IsSucceeded;
            Message = result.Result.Message;
            var loginDto = new Login
            {
                NationalCode = _authHelper.GetAccountInfo().NationalCode,
                Password = _authHelper.GetAccountInfo().Password
            };
            _accountApplication.Logout();
            _accountApplication.Login(loginDto);
            return RedirectToPage("./Profile");
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
