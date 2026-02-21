using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Semester;

namespace StudyManagement.Domain.SemesterAgg
{
    public interface ISemesterRepository : IRepositoryBase<long , Semester>
    {
        Semester GetCurrent();
        SemesterViewModel GetCurrentSemester();
        Semester GetByYearAndMidYear(int year, int midYear, long universityId);

        List<SemesterViewModel> GetSemesters();
        int GetSemesterCodeBy(long semesterId);
    }
}
