using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using AccountManagement.Domain.RoleAgg;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ClassModel : PageModel
    {
        private readonly IClassQuery _classQuery;
        private readonly ICourseQuery _courseQuery;
        private readonly IAuthHelper _authHelper;
        public List<ClassQueryModel> Classes;
        public string CourseName;

        //classes.ForEach(item =>
        //{
        //    item.ProfessorFullName = accounts.FirstOrDefault(x => x.Id == item.ProfessorId)?.FullName;
        //});

        public ClassModel(IClassQuery classQuery, ICourseQuery courseQuery, IAuthHelper authHelper)
        {
            _classQuery = classQuery;
            _courseQuery = courseQuery;
            _authHelper = authHelper;
        }

        public IActionResult OnGet(long courseId)
        {
            if (!_authHelper.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (_authHelper.CurrentAccountRole() == Roles.Professor)
            {
                return RedirectToPage("/Index", new { area = "Administration" });
            }
            Classes = _classQuery.GetClassesByCourseId(courseId);
            CourseName = _courseQuery.GetCourseNameById(courseId);
            return Page();
        }
    }
}
