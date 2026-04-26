using _01_Framework.Domain;

namespace StudyManagement.Domain.ClassAgg
{
    public interface IClassTemplateRepository : IRepositoryBase<long, ClassTemplate>
    {
        ClassTemplate GetBy(long courseId, long professorId);
        long GetIdBy(long courseId, long professorId);
    }
}
