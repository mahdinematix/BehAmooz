using _01_Framework.Application;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Application
{
    public class ClassApplication : IClassApplication
    {
        private readonly IClassRepository _classRepository;
        private readonly IAuthHelper _authHelper;

        public ClassApplication(IClassRepository classRepository, IAuthHelper authHelper)
        {
            _classRepository = classRepository;
            _authHelper = authHelper;
        }

        public OperationResult Create(CreateClass command)
        {
            var operation = new OperationResult();
            if (_classRepository.Exists(x=>x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startTime = Convert.ToInt32(command.StartTime.Substring(0,2));
            var endTime = Convert.ToInt32(command.EndTime.Substring(0, 2));
            if (startTime >= endTime)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
            }

            long professorId;
            if (_authHelper.CurrentAccountRole() == "2")
            {
                professorId = _authHelper.CurrentAccountId();
            }
            else
            {
                professorId = command.ProfessorId;
            }
            var classs = new Class(command.Code, command.StartTime, command.EndTime,
                command.CourseId, command.Day,professorId);
            _classRepository.Create(classs);
            _classRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditClass command)
        {
            var operation = new OperationResult();
            var classs = _classRepository.GetBy(command.Id);
            if (classs == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_classRepository.Exists(x=>x.Code==command.Code && x.Id != command.Id && x.CourseId == command.CourseId))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            if (command.StartTime == command.EndTime)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
            }

            classs.Edit(command.Code,command.StartTime,command.EndTime,command.CourseId, command.Day);
            _classRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Activate(long id)
        {
            var operation = new OperationResult();
            var classs = _classRepository.GetBy(id);
            if (classs == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            classs.Activate();
            _classRepository.Save();
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id)
        {
            var operation = new OperationResult();
            var classs = _classRepository.GetBy(id);
            if (classs == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            classs.DeActivate();
            _classRepository.Save();
            return operation.Succeed();
        }

        public EditClass GetDetails(long id)
        {
            return _classRepository.GetDetails(id);
        }

        public List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId)
        {
            return _classRepository.Search(searchModel, courseId);
        }

        public List<ClassViewModel> GetClasses()
        {
            return _classRepository.GetClasses();
        }

        public ClassViewModel GetClassById(long id)
        {
            return _classRepository.GetClassById(id);
        }
    }
}
