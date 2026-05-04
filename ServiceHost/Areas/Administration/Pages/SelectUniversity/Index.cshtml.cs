using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Pages.SelectUniversity
{
    public class IndexModel : UserContextPageModel
    {
        private readonly IUniversityApplication _universityApplication;

        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public SelectUniversityDto Command;

        public IndexModel(IUniversityApplication universityApplication, IAuthHelper authHelper):base(authHelper)
        {
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

            if (CurrentAccountRole!=Roles.SuperAdministrator)
            {
                return RedirectToPage("/Course/Index", new { area = "Administration", universityId = CurrentAccountUniversityId });
            }

            UniTypes = GetUniTypes();
            Unis = GetUnis();

            return Page();
        }


        private List<SelectListItem> GetUnis(int typeId = 0)
        {

            List<SelectListItem> lstUnis = _universityApplication.GetActiveUniversitiesByTypeId(typeId).Select(n =>
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
