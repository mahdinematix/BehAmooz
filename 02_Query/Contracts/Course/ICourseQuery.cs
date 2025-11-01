namespace _02_Query.Contracts.Course
{
    public interface ICourseQuery
    {
        List<CourseQueryModel> Search(CourseSearchModel searchModel);
        string GetCourseNameById(long courseId);
    }
}
