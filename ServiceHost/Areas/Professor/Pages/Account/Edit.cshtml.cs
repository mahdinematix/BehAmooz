using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Professor.Pages.Account
{
    public class EditModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IUniversityApplication _universityApplication;
        public List<SelectListItem> Unis { get; set; }
        public List<SelectListItem> UniTypes { get; set; }
        [BindProperty] public EditAccount Command { get; set; }

        [TempData] public string Message { get; set; }

        public EditModel(IAccountApplication accountApplication, IAuthHelper authHelper, IUniversityApplication universityApplication) : base(authHelper)
        {
            _accountApplication = accountApplication;
            _universityApplication = universityApplication;
        }

        public IActionResult OnGet(long id)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Command = _accountApplication.GetDetails(id);
            UniTypes = GetUniTypes();
            Unis = GetUnis(Command.UniversityType);

            return Page();
        }

        public IActionResult OnPost(EditAccount command)
        {

            var result = _accountApplication.Edit(command);
            if (result.Result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Result.Message;
            UniTypes = GetUniTypes();
            Unis = GetUnis(Command.UniversityType);
            return Page();
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
