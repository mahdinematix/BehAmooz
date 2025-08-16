using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class EditModel : PageModel
    {
        private readonly ICourseApplication _courseApplication;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;
        public EditCourse Command;
        [TempData] public string Message { get; set; }
        public EditModel(ICourseApplication courseApplication)
        {
            _courseApplication = courseApplication;
        }

        public void OnGet(long id)
        {
            Command = _courseApplication.GetDetails(id);
            UniTypes = GetUniTypes();
            Unis = GetUnis();
        }

        public IActionResult OnPost(EditCourse command)
        {
            var result = _courseApplication.Edit(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Edit", new {id = command.Id});
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
