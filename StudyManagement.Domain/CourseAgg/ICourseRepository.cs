using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Course;

namespace StudyManagement.Domain.CourseAgg
{
    public interface ICourseRepository : IRepositoryBase<long, Course>
    {
        List<CourseViewModel> Search(CourseSearchModel searchModel);
        EditCourse GetDetails(long id);
        List<CourseViewModel> GetCourses();
    }
}