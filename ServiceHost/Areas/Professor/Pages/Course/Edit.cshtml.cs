using _01_Framework.Application;
using Microsoft.AspNetCore.Mvc;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Application.Contracts.Semester;

namespace ServiceHost.Areas.Professor.Pages.Course
{
    public class EditModel : UserContextPageModel
    {
        private readonly ICourseApplication _courseApplication;
        private readonly ISemesterApplication _semesterApplication;
        [BindProperty] public EditCourse Command { get; set; }
        public List<SemesterViewModel> SemesterCodes { get; set; }
        public long UniversityId { get; set; }

        [TempData] public string Message { get; set; }
        public EditModel(ICourseApplication courseApplication, IAuthHelper authHelper, ISemesterApplication semesterApplication):base(authHelper)
        {
            _courseApplication = courseApplication;
            _semesterApplication = semesterApplication;
        }

        public IActionResult OnGet(long id, long universityId)
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
            SemesterCodes = _semesterApplication.GetSemestersByUniversityId(universityId);
            Command = _courseApplication.GetDetails(id);
            return Page();
        }

        public IActionResult OnPost(EditCourse command)
        {
            var result = _courseApplication.Edit(command, CurrentAccountId);
            if (result.IsSucceeded)
            {
                return RedirectToPage("./Index");
            }
            Message = result.Message;

            return Page();
        }
    }
}
