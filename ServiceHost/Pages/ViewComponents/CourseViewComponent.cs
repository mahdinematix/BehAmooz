using _02_Query.Contracts.Course;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly ICourseQuery _courseQuery;

        public CourseViewComponent(ICourseQuery courseQuery)
        {
            _courseQuery = courseQuery;
        }

        public IViewComponentResult Invoke()
        {
            var courses = _courseQuery.GetCourses();
            return View(courses);
        }
    }
}
