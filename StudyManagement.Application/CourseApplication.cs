using _01_Framework.Application;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Application
{
    public class CourseApplication : ICourseApplication
    {
        private readonly ICourseRepository _courseRepository;

        public CourseApplication(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public OperationResult Create(CreateCourse command)
        {
            var operation = new OperationResult();
            if (_courseRepository.Exists(x => x.Name == command.Name && x.Major == command.Major && x.EducationLevel == command.EducationLevel))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }
            if (_courseRepository.Exists(x => x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }


            var course = new Course(command.Name, command.NumberOfUnit, command.CourseKind, command.Code, command.Major,command.UniversityType,command.University,command.Price,command.EducationLevel);
            _courseRepository.Create(course);
            _courseRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Edit(EditCourse command)
        {
            var operation = new OperationResult();
            var course = _courseRepository.GetBy(command.Id);
            if (course == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_courseRepository.Exists(x => x.Name == command.Name && x.Major == command.Major && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }


            course.Edit(command.Name, command.NumberOfUnit, command.CourseKind, command.Code, command.Major,command.UniversityType,command.University,command.Price, command.EducationLevel);
            _courseRepository.Save();
            return operation.Succeed();
        }

        public OperationResult Activate(long id)
        {
            var operation = new OperationResult();
            var course = _courseRepository.GetBy(id);
            if (course == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            course.Activate();
            _courseRepository.Save();
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id)
        {
            var operation = new OperationResult();
            var course = _courseRepository.GetBy(id);
            if (course == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }
            course.DeActivate();
            _courseRepository.Save();
            return operation.Succeed();
        }

        public List<CourseViewModel> Search(CourseSearchModel searchModel)
        {
            return _courseRepository.Search(searchModel);
        }

        public EditCourse GetDetails(long id)
        {
            return _courseRepository.GetDetails(id);
        }

        public List<CourseViewModel> GetCourses()
        {
            return _courseRepository.GetCourses();
        }

        public CourseViewModel GetByCourseId(long courseId)
        {
            return _courseRepository.GetByCourseId(courseId);
        }
    }
}