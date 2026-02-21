using _01_Framework.Application;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Domain.SemesterAgg;

namespace StudyManagement.Application
{
    public class SemesterApplication : ISemesterApplication
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IUniversityApplication _universityApplication;

        public SemesterApplication(ISemesterRepository semesterRepository, IUniversityApplication universityApplication)
        {
            _semesterRepository = semesterRepository;
            _universityApplication = universityApplication;
        }

        public OperationResult Define(DefineSemester command)
        {
            var operation = new OperationResult();
            var existedSemester = _semesterRepository
                .GetByYearAndMidYear(command.Year, command.MidYear, command.UniversityId);

            if (existedSemester != null)
            {
                var currentSemester = _semesterRepository.GetCurrent();
                if (currentSemester != null && currentSemester.Id != existedSemester.Id)
                    currentSemester.UnsetCurrent();

                existedSemester.SetAsCurrent();
                _semesterRepository.Save();

                return operation.Succeed();
            }

           
            var current = _semesterRepository.GetCurrent();
            if (current != null)
                current.UnsetCurrent();

            var semester = new Semester(command.MidYear, command.Year,command.UniversityId);
            semester.SetAsCurrent();

            _semesterRepository.Create(semester);
            _semesterRepository.Save();
            _universityApplication.SetCurrentSemesterId(command.UniversityId, semester.Id);

            return operation.Succeed();


        }

        public SemesterViewModel GetCurrentSemester()
        {
            return _semesterRepository.GetCurrentSemester();
        }

        public List<SemesterViewModel> GetSemesters()
        {
            return _semesterRepository.GetSemesters();
        }

        public int GetSemesterByCode(long semesterId)
        {
            return _semesterRepository.GetSemesterCodeBy(semesterId);
        }
    }
}
