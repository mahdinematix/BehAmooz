using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.LogContracts;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Domain.SemesterAgg;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Application
{
    public class UniversityApplication : IUniversityApplication 
    {
        private readonly IUniversityRepository _universityRepository;
        private readonly ILogApplication _logApplication;
        private readonly ISemesterRepository _semesterRepository;

        public UniversityApplication(IUniversityRepository universityRepository, ILogApplication logApplication, ISemesterRepository semesterRepository)
        {
            _universityRepository = universityRepository;
            _logApplication = logApplication;
            _semesterRepository = semesterRepository;
        }

        public OperationResult Define(DefineUniversity command)
        {
            var operation = new OperationResult();
            if (_universityRepository.Exists(x=>x.Type ==command.Type && x.Name == command.Name))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var university = new University(command.Name, command.Type);
            _universityRepository.Create(university);
            _universityRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditUniversity command)
        {
            var operation = new OperationResult();
            var university = _universityRepository.GetBy(command.Id);
            if (university == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            if (_universityRepository.Exists(x => x.Type == command.Type && x.Name == command.Name && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            university.Edit(command.Name, command.Type);
            _universityRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Activate(long id)
        {
            var operation = new OperationResult();
            var university = _universityRepository.GetBy(id);
            if (university == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            university.Activate();
            _universityRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Deactivate(long id)
        {
            var operation = new OperationResult();
            var university = _universityRepository.GetBy(id);
            if (university == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            university.Deactivate();
            _universityRepository.Save();
            return operation.Succeed();
        }

        public List<UniversityViewModel> Search(UniversitySearchModel searchModel)
        {
            return _universityRepository.Search(searchModel);
        }

        public EditUniversity GetDetails(long id)
        {
            return _universityRepository.GetDetails(id);
        }

        public List<UniversityViewModel> GetUniversitiesByType(int typeId)
        {
            return _universityRepository.GetUniversitiesByType(typeId);
        }

        public OperationResult SetCurrentSemesterId(long id, long semesterId, long currentAccountId)
        {
            var operation = new OperationResult();
            var type = _universityRepository.GetTypeByUniversityId(id);
            var university = _universityRepository.GetBy(id);
            if (university == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            university.SetCurrentSemester(semesterId);
            var semesterCode = _semesterRepository.GetSemesterCodeBy(semesterId);
            _universityRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Define,
                TargetId = university.Id,
                TargetType = TargetTypes.University,
                Description = type == 2
                    ? $"تنظیم ترم {semesterCode} برای همه دانشگاه‌های آزاد"
                    : $"تنظیم ترم {semesterCode}"
            });
            return operation.Succeed();
        }

        public long GetCurrentSemesterId(long id)
        {
            return _universityRepository.GetCurrentSemesterId(id);
        }

        public string GetNameBy(long id)
        {
            return _universityRepository.GetNameBy(id);
        }

        public List<UniversityViewModel> GetActiveUniversities()
        {
            return _universityRepository.GetActiveUniversities();
        }

        public List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId)
        {
            return _universityRepository.GetActiveUniversitiesByTypeId(typeId);
        }

        public int GetTypeByUniversityId(long universityId)
        {
            return _universityRepository.GetTypeByUniversityId(universityId);
        }

        public List<long> GetUniversityIdsByType(int typeId)
        {
            return _universityRepository.GetUniversityIdsByType(typeId);
        }
    }
}
