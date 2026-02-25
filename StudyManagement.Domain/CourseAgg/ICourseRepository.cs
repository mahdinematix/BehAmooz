using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Course;

namespace StudyManagement.Domain.CourseAgg
{
    public interface ICourseRepository : IRepositoryBase<long, Course>
    {
        List<CourseViewModel> Search(CourseSearchModel searchModel, long universityId, long currentAccountId, string currentAccountRole);
        EditCourse GetDetails(long id);
        List<CourseViewModel> GetCourses();
        CourseViewModel GetById(long courseId);
    }
}