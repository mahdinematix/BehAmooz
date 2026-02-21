using _01_Framework.Domain;
using StudyManagement.Application.Contracts.University;

namespace StudyManagement.Domain.UniversityAgg
{
    public interface IUniversityRepository : IRepositoryBase<long, University>
    {
        List<UniversityViewModel> Search(UniversitySearchModel searchModel);
        EditUniversity GetDetails(long id);
        List<UniversityViewModel> GetUniversitiesByType(int type);
        long GetCurrentSemesterId(long id);
        string GetNameBy(long id);
        List<UniversityViewModel> GetActiveUniversities();
        List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId);
    }
}
