using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Professor.Pages.Course
{
    public class CreateModel : UserContextPageModel
    {
        [BindProperty]
        public long UniversityId { get; set; }

        [BindProperty]
        public CreateCourse Command { get; set; }
        private readonly ICourseApplication _courseApplication;
        
        public CreateModel(ICourseApplication courseApplication, IAuthHelper authHelper):base(authHelper)
        {
            _courseApplication = courseApplication;
        }

        [TempData] public string Message { get; set; }
        public IActionResult OnGet(long universityId)
        {
            if (CurrentAccountStatus == Statuses.Waiting)
            {
                return RedirectToPage("/NotConfirmed");
            }

            if (CurrentAccountStatus == Statuses.Rejected)
            {
                return RedirectToPage("/Reject");
            }
            UniversityId = universityId;
           
            return Page();
        }

        public IActionResult OnPost(CreateCourse command)
        {
            command.UniversityId = UniversityId;
            var result = _courseApplication.Create(command, CurrentAccountId);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;
            return RedirectToPage("./Create");
        }

    }
}
