using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class IndexModel : PageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly IAuthHelper _authHelper;

        public CourseSearchModel SearchModel;
        public List<CourseViewModel> Courses;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;

        public IndexModel(ICourseApplication courseApplication, IAuthHelper authHelper)
        {
            _courseApplication = courseApplication;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(CourseSearchModel searchModel)
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
            Courses = _courseApplication.Search(searchModel);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnGetActivate(long id)
        {
            _courseApplication.Activate(id);
            return RedirectToPage("./Index");
        }

        public IActionResult OnGetDeActivate(long id)
        {
            _courseApplication.DeActivate(id);
            return RedirectToPage("./Index");
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
