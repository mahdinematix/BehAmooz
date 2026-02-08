using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.Log;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Application
{
    public class CourseApplication : ICourseApplication
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogApplication _logApplication; 
        private readonly IAuthHelper _authHelper;
        

        public CourseApplication(ICourseRepository courseRepository, ILogApplication logApplication, IAuthHelper authHelper)
        {
            _courseRepository = courseRepository;
            _logApplication = logApplication;
            _authHelper = authHelper;
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
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Create,
                TargetId = course.Id,
                TargetType = TargetTypes.Course
            });
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

            var oldName = course.Name;
            var oldPrice = course.Price;
            var oldMajor = course.Major;
            var oldNumberOfUnit = course.NumberOfUnit;
            var oldUniversity = course.University;
            var oldUniversityType = course.UniversityType;
            var oldEducationLevel = course.EducationLevel;
            var oldCourseKind = course.CourseKind;
            var oldCode = course.Code;


            course.Edit(command.Name, command.NumberOfUnit, command.CourseKind, command.Code, command.Major,command.UniversityType,command.University,command.Price, command.EducationLevel);
            _courseRepository.Save();

            if (!(oldName == command.Name && oldPrice == command.Price && oldMajor == command.Major &&
                  oldNumberOfUnit == command.NumberOfUnit && oldUniversity == command.University &&
                  oldUniversityType == command.UniversityType && oldEducationLevel == command.EducationLevel &&
                  oldCode == command.Code && oldCourseKind == command.CourseKind))
            {


                var changes = new List<string>();

                if (oldName != command.Name)
                    changes.Add($"نام از «{oldName}» به «{command.Name}»");

                if (oldPrice != command.Price)
                    changes.Add($"قیمت از «{oldPrice.ToMoney()}» به «{command.Price.ToMoney()}»");

                if (oldMajor != command.Major)
                    changes.Add($"رشته از «{oldMajor}» به «{command.Major}»");

                if (oldNumberOfUnit != command.NumberOfUnit)
                    changes.Add($"تعداد واحد از «{oldNumberOfUnit}» به «{command.NumberOfUnit}»");

                if (oldUniversity != command.University)
                    changes.Add(
                        $"دانشگاه از «{Universities.GetName(oldUniversity)}» به «{Universities.GetName(command.University)}»");

                if (oldUniversityType != command.UniversityType)
                    changes.Add(
                        $"نوع دانشگاه از «{UniversityTypes.GetName(oldUniversityType)}» به «{UniversityTypes.GetName(command.UniversityType)}»");

                if (oldEducationLevel != command.EducationLevel)
                    changes.Add($"مقطع از «{oldEducationLevel}» به «{command.EducationLevel}»");

                if (oldCode != command.Code)
                    changes.Add($"کد از «{oldCode}» به «{command.Code}»");

                if (oldCourseKind != command.CourseKind)
                    changes.Add($"نوع درس از «{oldCourseKind}» به «{command.CourseKind}»");



                var description = string.Join(" | ", changes);

                _logApplication.Create(new CreateLog
                {
                    AccountId = _authHelper.CurrentAccountId(),
                    Operation = Operations.Edit,
                    TargetId = course.Id,
                    TargetType = TargetTypes.Course,
                    Description = description
                });
            }

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
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Activate,
                TargetId = course.Id,
                TargetType = TargetTypes.Course,
            });
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
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Deactivate,
                TargetId = course.Id,
                TargetType = TargetTypes.Course,
            });
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