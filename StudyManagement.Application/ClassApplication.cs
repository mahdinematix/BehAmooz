using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using LogManagement.Application.Contracts.LogContracts;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Application
{
    public class ClassApplication : IClassApplication
    {
        private readonly IClassRepository _classRepository;
        private readonly IClassTemplateRepository _classTemplateRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ILogApplication _logApplication;
        private readonly IAccountApplication _accountApplication;

        public ClassApplication(IClassRepository classRepository, ISessionRepository sessionRepository, ILogApplication logApplication, IAccountApplication accountApplication, IClassTemplateRepository classTemplateRepository)
        {
            _classRepository = classRepository;
            _sessionRepository = sessionRepository;
            _logApplication = logApplication;
            _accountApplication = accountApplication;
            _classTemplateRepository = classTemplateRepository;
        }

        public OperationResult Create(CreateClass command, long currentAccountId)
        {
            var operation = new OperationResult();

            if (_classRepository.Exists(x => x.Code == command.Code))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startTimeHour = Convert.ToInt32(command.StartTime.Substring(0, 2));
            var endTimeHour = Convert.ToInt32(command.EndTime.Substring(0, 2));
            var startTimeMinute = Convert.ToInt32(command.StartTime.Substring(3, 2));
            var endTimeMinute = Convert.ToInt32(command.EndTime.Substring(3, 2));

            if (startTimeHour > endTimeHour)
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);

            if (startTimeHour == endTimeHour && startTimeMinute >= endTimeMinute)
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);

            var template = _classTemplateRepository.GetBy(command.CourseId, command.ProfessorId);
            if (template == null)
            {
                template = new ClassTemplate(command.CourseId, command.ProfessorId);
                _classTemplateRepository.Create(template);
                _classTemplateRepository.Save();
            }

            if (_classRepository.ExistsForProfessorAtTime(command.ProfessorId, command.Day, command.StartTime))
                return operation.Failed(ApplicationMessages.AClassExistsWithTheStartTime);

            var classs = new Class(command.Code, command.StartTime, command.EndTime, command.Day, template.Id);

            _classRepository.Create(classs);
            _classRepository.Save();

            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Create,
                TargetId = classs.Id,
                TargetType = TargetTypes.Class
            });

            return operation.Succeed();
        }

        public OperationResult Edit(EditClass command, long currentAccountId)
        {
            var operation = new OperationResult();

            var classs = _classRepository.GetBy(command.Id);
            if (classs == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            if (_classRepository.Exists(x => x.Code == command.Code && x.Id != command.Id))
                return operation.Failed(ApplicationMessages.DuplicatedRecord);

            var startTimeHour = Convert.ToInt32(command.StartTime.Substring(0, 2));
            var endTimeHour = Convert.ToInt32(command.EndTime.Substring(0, 2));
            var startTimeMinute = Convert.ToInt32(command.StartTime.Substring(3, 2));
            var endTimeMinute = Convert.ToInt32(command.EndTime.Substring(3, 2));

            if (startTimeHour > endTimeHour)
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);

            if (startTimeHour == endTimeHour && startTimeMinute >= endTimeMinute)
                return operation.Failed(ApplicationMessages.StartTimeAndEndTimeHaveInterference);

            var template = _classTemplateRepository.GetBy(command.CourseId, command.ProfessorId);
            if (template == null)
            {
                template = new ClassTemplate(command.CourseId, command.ProfessorId);
                _classTemplateRepository.Create(template);
                _classTemplateRepository.Save();
            }

            if (_classRepository.ExistsForProfessorAtTime(command.ProfessorId, command.Day, command.StartTime, command.Id))
                return operation.Failed(ApplicationMessages.AClassExistsWithTheStartTime);

            var oldCode = classs.Code;
            var oldStartTime = classs.StartTime;
            var oldEndTime = classs.EndTime;
            var oldDay = classs.Day;
            var oldTemplateId = classs.ClassTemplateId;

            classs.Edit(command.Code, command.StartTime, command.EndTime, command.Day, template.Id);
            _classRepository.Save();

            if (!(oldCode == command.Code && oldStartTime == command.StartTime && oldEndTime == command.EndTime &&
                  oldDay == command.Day && oldTemplateId == template.Id))
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

                if (oldTemplateId != template.Id)
                    changes.Add($"تغییر قالب کلاس");

                var description = string.Join(" | ", changes);

                _logApplication.Create(new CreateLog
                {
                    AccountId = currentAccountId,
                    Operation = Operations.Edit,
                    TargetId = classs.Id,
                    TargetType = TargetTypes.Class,
                    Description = description
                });
            }

            return operation.Succeed();
        }

        public OperationResult Activate(long id, long currentAccountId)
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
                AccountId = currentAccountId,
                Operation = Operations.Activate,
                TargetId = classs.Id,
                TargetType = TargetTypes.Class,
            });
            return operation.Succeed();
        }

        public OperationResult DeActivate(long id, long currentAccountId)
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
                AccountId = currentAccountId,
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

        public OperationResult Copy(CopyClassTemplate command, long currentAccountId)
        {
            var operation = new OperationResult();

            var classFrom = _classRepository.GetBy(command.ClassId);
            var classTo = _classRepository.GetClassByCode(command.ClassCode);

            if (classFrom == null || classTo == null)
                return operation.Failed(ApplicationMessages.NotFoundRecord);

            var fromTemplateId = classFrom.ClassTemplateId;
            var toTemplateId = classTo.ClassTemplateId;

            if (!_sessionRepository.HasAnySessionsByClassTemplateId(fromTemplateId))
                return operation.Failed(ApplicationMessages.TheClassHasNotAnySessions);

            _sessionRepository.DeleteAllByClassTemplateId(toTemplateId);

            var sourceSessions = _sessionRepository.GetAllByClassTemplateIdForCopy(fromTemplateId);

            foreach (var s in sourceSessions)
            {
                var newSession = new Session(s.Number, s.Title, s.Video, s.Booklet, s.Description, toTemplateId);
                _sessionRepository.Create(newSession);
            }

            _sessionRepository.Save();

            _logApplication.Create(new CreateLog
            {
                AccountId = currentAccountId,
                Operation = Operations.Copy,
                TargetId = classFrom.Id,
                TargetType = TargetTypes.Class,
                Description = $"به کلاس {classTo.Code}"
            });

            return operation.Succeed();
        }

        public string GetClassCodeById(long id)
        {
            return _classRepository.GetClassCodeById(id);
        }

        public List<ClassViewModel> GetClassesForCopy(long classId)
        {
            return _classRepository.GetClassesForCopy(classId);
        }
        public ClassInfoForCopy GetClassInfoByClassCode(string classCode)
        {
            return _classRepository.GetClassInfoByClassCode(classCode);
        }

        public long GetTemplateIdByClassId(long classId)
        {
            return _classRepository.GetTemplateIdByClassId(classId);
        }
    }
}
