using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Course;

namespace ServiceHost.Areas.Administration.Pages.Course
{
    public class CreateModel : UserContextPageModel
    {
        public CreateCourse Command;
        private readonly ICourseApplication _courseApplication;

        [BindProperty] public long UniversityId { get; set; }
        
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
                return RedirectToPage("./Index" , new {universityId=UniversityId});
            }
            Message = result.Message;
            return RedirectToPage("./Create");
        }

    }
}
