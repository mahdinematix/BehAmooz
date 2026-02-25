using System.Collections;
using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Semester
{
    public interface ISemesterApplication
    {
        OperationResult Define(DefineSemester command, long currentAccountId);
        void DefineAutoSemester(long universityId);
        SemesterViewModel GetCurrentSemester(long universityId);
        List<SemesterViewModel> GetSemestersByUniversityId(long universityId);
        int GetSemesterByCode(long semesterId);
    }
}
