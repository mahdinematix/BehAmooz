using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using LogManagement.Application.Contracts.Log;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Application
{
    public class ClassApplication : IClassApplication
    {
        private readonly IClassRepository _classRepository;
        private readonly IAuthHelper _authHelper;
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogApplication _logApplication;
        private readonly IAccountApplication _accountApplication;

        public ClassApplication(IClassRepository classRepository, IAuthHelper authHelper, ISessionRepository sessionRepository, ILogApplication logApplication, IAccountApplication accountApplication)
        {
            _classRepository = classRepository;
            _authHelper = authHelper;
            _sessionRepository = sessionRepository;
            _logApplication = logApplication;
            _accountApplication = accountApplication;
        }

        public OperationResult Create(CreateClass command)
        {
            var operation = new OperationResult();
            if (_classRepository.Exists(x => x.Code == command.Code))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startTimeHour = Convert.ToInt32(command.StartTime.Substring(0, 2));
            var endTimeHour = Convert.ToInt32(command.EndTime.Substring(0, 2));
            var startTimeMinute = Convert.ToInt32(command.StartTime.Substring(3, 2));
            var endTimeMinute = Convert.ToInt32(command.EndTime.Substring(3, 2));
            if (startTimeHour > endTimeHour)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
            }
            if (startTimeHour == endTimeHour)
            {
                if (startTimeMinute >= endTimeMinute)
                {
                    return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
                }
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

            if (_classRepository.Exists(x => x.ProfessorId == professorId && x.StartTime == command.StartTime && x.Day == command.Day))
            {
                return operation.Failed(ApplicationMessages.AClassExistsWithTheStartTime);
            }
            var classs = new Class(command.Code, command.StartTime, command.EndTime,
                command.CourseId, command.Day, professorId);
            _classRepository.Create(classs);
            _classRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Create,
                TargetId = classs.Id,
                TargetType = TargetTypes.Class
            });
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

            if (_classRepository.Exists(x => x.Code == command.Code && x.Id != command.Id && x.CourseId == command.CourseId))
            {
                return operation.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var startTimeHour = Convert.ToInt32(command.StartTime.Substring(0, 2));
            var endTimeHour = Convert.ToInt32(command.EndTime.Substring(0, 2));
            var startTimeMinute = Convert.ToInt32(command.StartTime.Substring(3, 2));
            var endTimeMinute = Convert.ToInt32(command.EndTime.Substring(3, 2));
            if (startTimeHour > endTimeHour)
            {
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
            }
            if (startTimeHour == endTimeHour)
            {
                if (startTimeMinute >= endTimeMinute)
                {
                    return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);
                }
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

            if (_classRepository.Exists(x => x.ProfessorId == professorId && x.StartTime == command.StartTime && x.Day == command.Day && x.Id != command.Id))
            {
                return operation.Failed(ApplicationMessages.AClassExistsWithTheStartTime);
            }

            var oldCode = classs.Code;
            var oldStartTime = classs.StartTime;
            var oldEndTime = classs.EndTime;
            var oldDay = classs.Day;
            var oldProfessorId = classs.ProfessorId;

            classs.Edit(command.Code, command.StartTime, command.EndTime, command.CourseId, command.Day, professorId);
            _classRepository.Save();

            if (!(oldCode == command.Code && oldStartTime == command.StartTime && oldEndTime == command.EndTime && oldDay == command.Day && oldProfessorId == command.ProfessorId))
            {
                var changes = new List<string>();

                if (oldCode != command.Code)
                    changes.Add($"کد از «{oldCode}» به «{command.Code}»");

                if (oldStartTime != command.StartTime)
                    changes.Add($"ساعت شروع از «{oldStartTime}» به «{command.StartTime}»");

                if (oldEndTime != command.EndTime)
                    changes.Add($"ساعت پایان از «{oldEndTime}» به «{command.EndTime}»");

                if (oldDay != command.Day)
                    changes.Add($"روز از «{Days.GetName(oldDay)}» به «{Days.GetName(command.Day)}»");

                if (oldProfessorId != command.ProfessorId)
                    changes.Add($"استاد از «{_accountApplication.GetProfessorById(oldProfessorId)}» به «{_accountApplication.GetProfessorById(command.ProfessorId)}»");

                var description = string.Join(" | ", changes);

                _logApplication.Create(new CreateLog
                {
                    AccountId = _authHelper.CurrentAccountId(),
                    Operation = Operations.Edit,
                    TargetId = classs.Id,
                    TargetType = TargetTypes.Class,
                    Description = description
                });
            }

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
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Activate,
                TargetId = classs.Id,
                TargetType = TargetTypes.Class,
            });
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
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Deactivate,
                TargetId = classs.Id,
                TargetType = TargetTypes.Class,
            });
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

        public List<ClassViewModel> GetClasses(long classId)
        {
            return _classRepository.GetClasses(classId);
        }

        public ClassViewModel GetClassById(long id)
        {
            return _classRepository.GetClassById(id);
        }

        public OperationResult Copy(CopyClass command)
        {
            var operation = new OperationResult();
            var classThatFromCopy = _classRepository.GetBy(command.ClassId);
            var classThatToCopy = _classRepository.GetClassByCode(command.ClassCode);
            if (classThatFromCopy == null || classThatToCopy == null)
            {
                return operation.Failed(ApplicationMessages.NotFoundRecord);
            }

            var existingSessions = _sessionRepository.GetAllByClassIdForCopy(classThatToCopy.Id);
            foreach (var session in existingSessions)
            {
                _sessionRepository.Delete(session);
            }
            var classThatFromCopySessions = _sessionRepository.GetAllByClassIdForCopy(classThatFromCopy.Id);
            foreach (var session in classThatFromCopySessions)
            {
                var newSession = new Session(session.Number, session.Title, session.Video, session.Booklet, session.Description, session.ClassId);

                classThatToCopy.Sessions.Add(newSession);
            }
            _classRepository.Save();
            _logApplication.Create(new CreateLog
            {
                AccountId = _authHelper.CurrentAccountId(),
                Operation = Operations.Copy,
                TargetId = classThatFromCopy.Id,
                TargetType = TargetTypes.Class,
                Description = $"به کلاس {classThatToCopy.Code}"
            });
            return operation.Succeed();
        }

        public string GetClassCodeById(long id)
        {
            return _classRepository.GetClassCodeById(id);
        }

        public List<ClassViewModel> GetClassesForCopy(long classId, long courseId)
        {
            return _classRepository.GetClassesForCopy(classId, courseId);
        }

        public string GetCourseNameByClassId(long classId)
        {
            return _classRepository.GetCourseNameByClassId(classId);
        }
    }
}
