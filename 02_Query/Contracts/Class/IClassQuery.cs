using _02_Query.Contracts.Course;

namespace _02_Query.Contracts.Class
{
    public interface IClassQuery
    {
        List<ClassQueryModel> GetClassesByCourseId(long courseId);

        ClassQueryModel GetClassById(long classId);
        CourseQueryModel GetCourseNameAndPriceByClassId(long courseId);
    }
}
