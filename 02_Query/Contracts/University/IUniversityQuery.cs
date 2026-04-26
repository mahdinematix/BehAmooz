using StudyManagement.Application.Contracts.University;

namespace _02_Query.Contracts.University
{
    public interface IUniversityQuery
    {
        UniversityQueryModel GetById(long id);
        List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId);
    }
}
