using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Infrastructure.Configuration.Permissions;

namespace ServiceHost.Areas.Administration.Pages.University
{
    public class IndexModel : UserContextPageModel
    {
        private readonly IUniversityApplication _universityApplication;
        private readonly ISemesterApplication _semesterApplication;

        public UniversitySearchModel SearchModel;
        public List<UniversityViewModel> Universities;
        public List<SelectListItem> UniTypes;
        public List<SelectListItem> Unis;
        public SelectList SemesterCodes;

        public IndexModel(IUniversityApplication universityApplication, ISemesterApplication semesterApplication, IAuthHelper authHelper):base(authHelper)
        {
            _universityApplication = universityApplication;
            _semesterApplication = semesterApplication;
        }
        [NeedsPermissions(StudyPermissions.ListUniversity)]
        public IActionResult OnGet(UniversitySearchModel searchModel)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            Universities = _universityApplication.Search(searchModel);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            SemesterCodes = new SelectList(
                _semesterApplication.GetSemesters(),
                "Id",
                "Code"
            );
            return Page();
        }

        public IActionResult OnGetCreate()
        {
            var command = new DefineUniversity
            {
                UniTypes = UniversityTypes.List
            };
            return Partial("./Create",command);
        }

        [NeedsPermissions(StudyPermissions.CreateUniversity)]
        public IActionResult OnPostCreate(DefineUniversity command)
        {
            var result = _universityApplication.Define(command);
            return new JsonResult(result);
        }

        public IActionResult OnGetEdit(long id)
        {
            var university = _universityApplication.GetDetails(id);
            university.UniTypes = UniversityTypes.List;
            return Partial("Edit", university);
        }

        [NeedsPermissions(StudyPermissions.EditUniversity)]
        public IActionResult OnPostEdit(EditUniversity command)
        {
            var result = _universityApplication.Edit(command);
            return new JsonResult(result);
        }

        [NeedsPermissions(StudyPermissions.ActivateUniversity)]
        public IActionResult OnGetActivate(long id)
        {
            _universityApplication.Activate(id);
            return RedirectToPage("./Index");
        }

        [NeedsPermissions(StudyPermissions.DeactivateUniversity)]
        public IActionResult OnGetDeActivate(long id)
        {
            _universityApplication.Deactivate(id);
            return RedirectToPage("./Index");
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
