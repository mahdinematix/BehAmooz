using _01_Framework.Application;

namespace _02_Query.Contracts.Course
{
    public interface ICourseQuery
    {
        List<CourseQueryModel> Search(CourseSearchModel searchModel, AuthViewModel currentAccountInfo);
        string GetCourseNameById(long courseId);
    }
}
