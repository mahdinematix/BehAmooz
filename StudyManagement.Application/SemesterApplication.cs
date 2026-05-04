using _01_Framework.Application;
using _01_Framework.Infrastructure;
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

        public OperationResult Define(DefineSemester command, long currentAccountId)
        {
            var operation = new OperationResult();
            var type = _universityApplication.GetTypeByUniversityId(command.UniversityId);

            if (type == 2)
            {
                var targetUniversityIds = _universityApplication.GetUniversityIdsByType(2);
                
                foreach (var uniId in targetUniversityIds)
                    ApplySemesterForUniversity(command.Year, command.MidYear, uniId, currentAccountId);
            }
            


            var existedSemester = _semesterRepository
                .GetByYearAndMidYear(command.Year, command.MidYear, command.UniversityId);

            if (existedSemester != null)
            {
                var currentSemester = _semesterRepository.GetCurrentByUniversityId(command.UniversityId);
                if (currentSemester != null && currentSemester.Id != existedSemester.Id)
                    currentSemester.UnsetCurrent();

                existedSemester.SetAsCurrent();
                _semesterRepository.Save();
                _universityApplication.SetCurrentSemesterId(command.UniversityId, existedSemester.Id, currentAccountId);

                return operation.Succeed();
            }
            var current = _semesterRepository.GetCurrentByUniversityId(command.UniversityId);
            if (current != null)
                current.UnsetCurrent();

            var semester = new Semester(command.MidYear, command.Year,command.UniversityId);
            semester.SetAsCurrent();

            _semesterRepository.Create(semester);
            _semesterRepository.Save();
            _universityApplication.SetCurrentSemesterId(command.UniversityId, semester.Id, currentAccountId);
            return operation.Succeed();
        }

        public void DefineAutoSemester(long universityId)
        {
            var year = DateTime.Now.ToFarsiYear();
            var month = DateTime.Now.Month;
            int midYear;
            if (month ==1 || (month >8 && month <=12))
            {
                midYear = MidYears.First;
            }
            else if(month >1 && month <=6)
            {
                midYear = MidYears.Second;
            }
            else
            {
                midYear = MidYears.Summer;
            }
            var existedSemester = _semesterRepository
                .GetByYearAndMidYear(year, midYear, universityId);

            if (existedSemester != null)
            {
                var currentSemester = _semesterRepository.GetCurrentByUniversityId(universityId);
                if (currentSemester != null && currentSemester.Id != existedSemester.Id)
                    currentSemester.UnsetCurrent();

                existedSemester.SetAsCurrent();
                _semesterRepository.Save();
            }

            var current = _semesterRepository.GetCurrentByUniversityId(universityId);
            if (current != null)
                current.UnsetCurrent();

            var semester = new Semester(midYear, year, universityId);
            semester.SetAsCurrent();

            _semesterRepository.Create(semester);
            _semesterRepository.Save();
            _universityApplication.SetCurrentSemesterId(universityId, semester.Id, 0);
            }

        public SemesterViewModel GetCurrentSemester(long universityId)
        {
            return _semesterRepository.GetCurrentSemester(universityId);
        }

        public List<SemesterViewModel> GetSemestersByUniversityId(long universityId)
        {
            return _semesterRepository.GetSemestersByUniversityId(universityId);
        }

        private void ApplySemesterForUniversity(int year, int midYear, long universityId, long currentAccountId)
        {
            var existedSemester = _semesterRepository.GetByYearAndMidYear(year, midYear, universityId);

            if (existedSemester != null)
            {
                var currentSemester = _semesterRepository.GetCurrentByUniversityId(universityId);
                if (currentSemester != null && currentSemester.Id != existedSemester.Id)
                    currentSemester.UnsetCurrent();

                existedSemester.SetAsCurrent();
                _semesterRepository.Save();

                _universityApplication.SetCurrentSemesterId(universityId, existedSemester.Id, currentAccountId);
                return;
            }

            var current = _semesterRepository.GetCurrentByUniversityId(universityId);
            if (current != null) current.UnsetCurrent();

            var semester = new Semester(midYear, year, universityId);
            semester.SetAsCurrent();

            _semesterRepository.Create(semester);
            _semesterRepository.Save();

            _universityApplication.SetCurrentSemesterId(universityId, semester.Id, currentAccountId);
        }
    }
}
