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

        public CourseSearchModel SearchModel;
        public List<CourseViewModel> Courses;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;

        public IndexModel(ICourseApplication courseApplication)
        {
            _courseApplication = courseApplication;
        }

        public void OnGet(CourseSearchModel searchModel)
        {
            Courses = _courseApplication.Search(searchModel);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
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
