using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Professor.Pages
{
    public class ProfileModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IUniversityApplication _universityApplication;

        public bool ShowSomeFields { get; set; }
        public List<SelectListItem> Unis { get; set; }
        public List<SelectListItem> UniTypes { get; set; }
        [BindProperty] public EditAccount Command { get; set; }

        [TempData] public bool IsSucceeded { get; set; }
        [TempData] public string Message { get; set; }

        public ProfileModel(IAccountApplication accountApplication, IAuthHelper authHelper, IUniversityApplication universityApplication):base(authHelper)
        {
            _accountApplication = accountApplication;
            _universityApplication = universityApplication;
        }

        public IActionResult OnGet()
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            var id = CurrentAccountId;
            Command = _accountApplication.GetDetails(id);
            UniTypes = GetUniTypes();
            Unis = GetUnis(Command.UniversityType);
            ShowSomeFields = CurrentAccountStatus != Statuses.Confirmed;
            return Page();
        }

        public IActionResult OnPost(EditAccount command)
        {
            var result = _accountApplication.Edit(command);
            IsSucceeded = result.Result.IsSucceeded;
            if (!IsSucceeded)
            {
                Message = result.Result.Message;
                UniTypes = GetUniTypes();
                Unis = GetUnis(Command.UniversityType);
                return Page();

            }
            var loginDto = new Login
            {
                NationalCode = CurrentAccountNationalCode,
                Password = CurrentAccountPassword
            };
            _accountApplication.Logout();
            _accountApplication.Login(loginDto);
            UniTypes = GetUniTypes();
            Unis = GetUnis(Command.UniversityType);
            return Page();
        }

        public IActionResult OnGetChangePassword(long id)
        {
            var command = new ChangePassword { Id = id };
            return Partial("ChangePassword", command);
        }

        public JsonResult OnPostChangePassword(ChangePassword command)
        {
            var result = _accountApplication.ChangePasswordByUser(command);
            return new JsonResult(result);
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
