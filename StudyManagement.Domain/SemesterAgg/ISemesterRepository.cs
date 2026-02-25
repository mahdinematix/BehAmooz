using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Semester;

namespace StudyManagement.Domain.SemesterAgg
{
    public interface ISemesterRepository : IRepositoryBase<long , Semester>
    {
        Semester GetCurrentByUniversityId(long universityId);
        SemesterViewModel GetCurrentSemester(long universityId);
        Semester GetByYearAndMidYear(int year, int midYear, long universityId);

        List<SemesterViewModel> GetSemestersByUniversityId(long universityId);
        int GetSemesterCodeBy(long semesterId);
    }
}
