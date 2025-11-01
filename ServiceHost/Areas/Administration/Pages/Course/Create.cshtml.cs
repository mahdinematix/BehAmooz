using System.Runtime.CompilerServices;
using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class CreateModel : PageModel
    {
        public CreateCourse Command;
        private readonly ICourseApplication _courseApplication;
        private readonly IAuthHelper _authHelper;
        public List<SelectListItem> Unis;
        public List<SelectListItem> UniTypes;

        public CreateModel(ICourseApplication courseApplication, IAuthHelper authHelper)
        {
            _courseApplication = courseApplication;
            _authHelper = authHelper;
        }

        [TempData] public string Message { get; set; }
        public IActionResult OnGet()
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
            UniTypes = GetUniTypes();
            Unis = GetUnis();
            return Page();
        }

        public IActionResult OnPost(CreateCourse command)
        {
            var result = _courseApplication.Create(command);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Create");
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
