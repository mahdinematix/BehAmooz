using _01_Framework.Application;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Application
{
    public class UniversityApplication : IUniversityApplication 
    {
        private readonly IUniversityRepository _universityRepository;

        public UniversityApplication(IUniversityRepository universityRepository)
        {
            _universityRepository = universityRepository;
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

        public OperationResult SetCurrentSemesterId(long id, long semesterId)
        {
            var operation = new OperationResult();
            var university = _universityRepository.GetBy(id);
            if (university == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            university.SetCurrentSemester(semesterId);
            _universityRepository.Save();
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
    }
}
