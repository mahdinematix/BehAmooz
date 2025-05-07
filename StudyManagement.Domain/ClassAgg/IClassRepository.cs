using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Class;

namespace StudyManagement.Domain.ClassAgg
{
    public interface IClassRepository : IRepositoryBase<long,Class>
    {
        EditClass GetDetails(long id);
        List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId);
        List<ClassViewModel> GetClasses();
        ClassViewModel GetClassById(long id);
    }
}
