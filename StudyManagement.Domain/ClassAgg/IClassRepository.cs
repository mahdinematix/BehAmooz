using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Class;

namespace StudyManagement.Domain.ClassAgg
{
    public interface IClassRepository : IRepositoryBase<long,Class>
    {
        EditClass GetDetails(long id);
        List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId, long currentAccountId, string currentAccountRole);
        ClassViewModel GetClassById(long id);
        long GetTemplateIdByClassId(long classId);
        bool ExistsForProfessorAtTime(long professorId, int day, string startTime, long? excludeClassId = null);
    }
}
