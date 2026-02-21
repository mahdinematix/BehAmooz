using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Pages
{
    public class RegisterModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IUniversityApplication _universityApplication;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public RegisterAccount Command;
        [TempData] public string RegisterMessage { get; set; }

        public RegisterModel(IAccountApplication accountApplication, IAuthHelper authHelper, IUniversityApplication universityApplication) : base(authHelper)
        {
            _accountApplication = accountApplication;
            _universityApplication = universityApplication;
        }

        public IActionResult OnGet()
        {
            if (!IsAuthenticated)
            {
                Command = new RegisterAccount
                {
                    RoleId = 3
                };
                UniTypes = GetUniTypes();
                Unis = GetUnis();
            }

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

            List<SelectListItem> lstUnis = _universityApplication.GetUniversitiesByType(typeId).Select(n =>
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
