using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.LogContracts;
using LogManagement.Infrastructure.Configuration.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.University;

namespace ServiceHost.Areas.Administration.Pages.ActivityLog
{
    public class IndexModel : UserContextPageModel
    {
        private readonly ILogApplication _logApplication;
        private readonly IUniversityApplication _universityApplication;

        public LogSearchModel SearchModel;
        public List<LogViewModel> Logs;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public bool ShowUniversityFiled { get; set; }

        public IndexModel(ILogApplication logApplication, IAuthHelper authHelper, IUniversityApplication universityApplication) :base(authHelper)
        {
            _logApplication = logApplication;
            _universityApplication = universityApplication;
        }


        [NeedsPermissions(ActivityLogPermissions.ShowActivityLog)]
        public IActionResult OnGet(LogSearchModel searchModel)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            ShowUniversityFiled = CurrentAccountRole == Roles.SuperAdministrator;
            Logs = _logApplication.Search(searchModel, CurrentAccountUniversityId, CurrentAccountRole);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
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
