using System.Collections;
using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Semester
{
    public interface ISemesterApplication
    {
        OperationResult Define(DefineSemester command);
        SemesterViewModel GetCurrentSemester();
        List<SemesterViewModel> GetSemesters();
        int GetSemesterByCode(long semesterId);
    }
}
