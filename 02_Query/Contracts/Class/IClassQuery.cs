namespace _02_Query.Contracts.Class
{
    public interface IClassQuery
    {
        List<ClassQueryModel> GetClassesByCourseId(long courseId);

        ClassQueryModel GetClassById(long classId);
        string GetCourseNameByClassId(long classId);
    }
}
