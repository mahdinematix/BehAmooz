using _01_Framework.Application;
using _01_Framework.Infrastructure;
using LogManagement.Application.Contracts.LogContracts;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Domain.SemesterAgg;

namespace StudyManagement.Application
{
    public class CourseApplication : ICourseApplication
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ILogApplication _logApplication; 
        private readonly ISemesterRepository _semesterRepository;
        

        public CourseApplication(ICourseRepository courseRepository, ILogApplication logApplication, ISemesterRepository semesterRepository)
        {
            _courseRepository = courseRepository;
            _logApplication = logApplication;
            _semesterRepository = semesterRepository;
        }

        public OperationResult Create(CreateCourse command, long currentAccountId)
        {
            var operation = new OperationResult();
            if (_courseRepository.Exists(x => x.Name == command.Name && x.Major == command.Major && x.EducationLevel == command.EducationLevel && x.SemesterId == command.SemesterId))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }
            if (_courseRepository.Exists(x => x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var currentSemester = _semesterRepository.GetCurrentByUniversityId(command.UniversityId).Id;
            var course = new Course(command.Name, command.NumberOfUnit, command.CourseKind, command.Code, command.Major,command.Price,command.EducationLevel,currentSemester, command.UniversityId);
            _courseRepository.Create(course);
            _courseRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Create,
                TargetId = course.Id,
                TargetType = TargetTypes.Course
            });
            return operation.Succeed();
        }

        public OperationResult Edit(EditCourse command, long currentAccountId)
        {
            var operation = new OperationResult();
            var course = _courseRepository.GetBy(command.Id);
            if (course == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            if (_courseRepository.Exists(x => x.Name == command.Name && x.Major == command.Major && x.SemesterId == command.SemesterId && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var oldName = course.Name;
            var oldPrice = course.Price;
            var oldMajor = course.Major;
            var oldNumberOfUnit = course.NumberOfUnit;
            var oldEducationLevel = course.EducationLevel;
            var oldCourseKind = course.CourseKind;
            var oldCode = course.Code;
            var oldSemesterId = course.SemesterId;


            course.Edit(command.Name, command.NumberOfUnit, command.CourseKind, command.Code, command.Major,command.Price, command.EducationLevel, command.SemesterId);
            _courseRepository.Save();

            if (!(oldName == command.Name && oldPrice == command.Price && oldMajor == command.Major &&
                  oldNumberOfUnit == command.NumberOfUnit && oldEducationLevel == command.EducationLevel &&
                  oldCode == command.Code && oldCourseKind == command.CourseKind && oldSemesterId == command.SemesterId))
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

                if (oldEducationLevel != command.EducationLevel)
                    changes.Add($"مقطع از «{oldEducationLevel}» به «{command.EducationLevel}»");

                if (oldCode != command.Code)
                    changes.Add($"کد از «{oldCode}» به «{command.Code}»");

                if (oldCourseKind != command.CourseKind)
                    changes.Add($"نوع درس از «{oldCourseKind}» به «{command.CourseKind}»");

                if (oldSemesterId != command.SemesterId)
                {
                    changes.Add($"ترم از «{_semesterRepository.GetSemesterCodeBy(oldSemesterId)}» به «{_semesterRepository.GetSemesterCodeBy(command.SemesterId)}»");
                }


                var description = string.Join(" | ", changes);

                _logApplication.Create(new CreateLog
                {
                    AccountId = currentAccountId,
                    Operation = Operations.Edit,
                    TargetId = course.Id,
                    TargetType = TargetTypes.Course,
                    Description = description
                });
            }

            return operation.Succeed();
        }

        public OperationResult Activate(long id, long currentAccountId)
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
                AccountId = currentAccountId,
                Operation = Operations.Activate,
                TargetId = course.Id,
                TargetType = TargetTypes.Course,
            });
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id, long currentAccountId)
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
                AccountId = currentAccountId,
                Operation = Operations.Deactivate,
                TargetId = course.Id,
                TargetType = TargetTypes.Course,
            });
            return operation.Succeed();
        }

        public List<CourseViewModel> Search(CourseSearchModel searchModel, long universityId, long currentAccountId, string currentAccountRole)
        {
            return _courseRepository.Search(searchModel,universityId, currentAccountId, currentAccountRole);
        }

        public EditCourse GetDetails(long id)
        {
            return _courseRepository.GetDetails(id);
        }

        public List<CourseViewModel> GetCourses()
        {
            return _courseRepository.GetCourses();
        }

        public CourseViewModel GetById(long courseId)
        {
            return _courseRepository.GetById(courseId);
        }
    }
}