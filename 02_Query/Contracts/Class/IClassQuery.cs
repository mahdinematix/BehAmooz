using _02_Query.Contracts.Course;

namespace _02_Query.Contracts.Class
{
    public interface IClassQuery
    {
        List<ClassQueryModel> GetClassesByCourseId(long courseId);
        ClassQueryModel GetClassById(long classId);
        ClassQueryModel GetClassTemplateById(long classTemplateId);
    }
}
