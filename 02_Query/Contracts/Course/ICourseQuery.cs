namespace _02_Query.Contracts.Course
{
    public interface ICourseQuery
    {
        List<CourseQueryModel> GetCourses();
        string GetCourseNameById(long courseId);
    }
}
