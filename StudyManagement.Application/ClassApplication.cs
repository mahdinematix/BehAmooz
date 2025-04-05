using _01_Framework.Application;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Application
{
    public class ClassApplication : IClassApplication
    {
        private readonly IClassRepository _classRepository;

        public ClassApplication(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public OperationResult Create(CreateClass command)
        {
            var operation = new OperationResult();
            if (_classRepository.Exists(x=>x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            if (command.StartTime == command.EndTime)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeIsEqual);
            }


            var classs = new Class(command.Code, command.StartTime, command.EndTime,
                command.CourseId, command.Day);
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

            if (_classRepository.Exists(x=>x.Code==command.Code && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            if (command.StartTime == command.EndTime)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeIsEqual);
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

        public List<ClassViewModel> Search(ClassSearchModel searchModel)
        {
            return _classRepository.Search(searchModel);
        }
    }
}
