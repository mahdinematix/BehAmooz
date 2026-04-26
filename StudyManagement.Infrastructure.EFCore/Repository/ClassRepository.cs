using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class ClassRepository : RepositoryBase<long, Class>, IClassRepository
    {
        private readonly StudyContext _context;
        private readonly AccountContext _accountContext;
        private readonly IAuthHelper _authHelper;

        public ClassRepository(StudyContext context, AccountContext accountContext, IAuthHelper authHelper) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
            _authHelper = authHelper;
        }

        public EditClass GetDetails(long id)
        {
            return _context.Classes
                .Select(x => new EditClass
                {
                    Id = x.Id,
                    Code = x.Code,
                    EndTime = x.EndTime.ToShownTime(),
                    StartTime = x.StartTime.ToShownTime(),
                    Day = x.Day,
                    CourseId = x.Template.CourseId,
                    ProfessorId = x.Template.ProfessorId
                })
                .FirstOrDefault(x => x.Id == id);
        }

        public List<ClassViewModel> Search(ClassSearchModel searchModel, long courseId)
        {
            var accountId = _authHelper.CurrentAccountId();
            var role = _authHelper.CurrentAccountRole();

            var query =
                from c in _context.Classes
                join t in _context.ClassTemplates on c.ClassTemplateId equals t.Id
                join crs in _context.Courses on t.CourseId equals crs.Id
                select new ClassViewModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    DayId = c.Day,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,
                    IsActive = c.IsActive && crs.IsActive,
                    CreationDate = c.CreationDate.ToFarsi(),
                    ClassTemplateId = c.ClassTemplateId,
                    CourseId = crs.Id,
                    CourseName = crs.Name,
                    ProfessorId = t.ProfessorId,

                    SessionsCount = _context.Sessions.Count(s => s.ClassTemplateId == t.Id)
                };

            
            query = query.Where(x => x.CourseId == courseId);

            
            if (role == Roles.Professor)
                query = query.Where(x => x.ProfessorId == accountId);

            
            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                query = query.Where(x => x.Code.Contains(searchModel.Code));

            if (!string.IsNullOrWhiteSpace(searchModel.StartTime) && searchModel.StartTime != "0")
                query = query.Where(x => x.StartTime == searchModel.StartTime);

            if (searchModel.DayId > 0)
                query = query.Where(x => x.DayId == searchModel.DayId);

            if (searchModel.ProfessorId > 0)
                query = query.Where(x => x.ProfessorId == searchModel.ProfessorId);

            if (searchModel.IsActive)
                query = query.Where(x => !x.IsActive);

            var result = query.OrderByDescending(x => x.Id).ToList();

            var profIds = result.Select(x => x.ProfessorId).Distinct().ToList();
            var profDict = _accountContext.Accounts
                .Where(a => profIds.Contains(a.Id))
                .Select(a => new { a.Id, FullName = a.FirstName + " " + a.LastName })
                .ToDictionary(a => a.Id, a => a.FullName);

            result.ForEach(x =>
            {
                if (profDict.TryGetValue(x.ProfessorId, out var fn))
                    x.ProfessorFullName = fn;
            });

            return result;
        }

        public List<ClassViewModel> GetClasses(long classId)
        {
            return _context.Classes.Where(x => x.Id != classId).Select(x => new ClassViewModel
            {
                Id = x.Id,
                Code = x.Code
            }).ToList();
        }

        public ClassViewModel GetClassById(long id)
        {
            return (from c in _context.Classes
                join t in _context.ClassTemplates on c.ClassTemplateId equals t.Id
                join crs in _context.Courses on t.CourseId equals crs.Id
                where c.Id == id
                select new ClassViewModel
                {
                    Id = c.Id,
                    Code = c.Code,
                    DayId = c.Day,
                    StartTime = c.StartTime,
                    EndTime = c.EndTime,

                    ClassTemplateId = c.ClassTemplateId,
                    CourseId = crs.Id,
                    CourseName = crs.Name,
                    ProfessorId = t.ProfessorId,

                    SessionsCount = _context.Sessions.Count(s => s.ClassTemplateId == t.Id)
                }).FirstOrDefault();
        }

        public string GetClassCodeById(long id)
        {
            return _context.Classes.FirstOrDefault(x => x.Id == id).Code;
        }

        public List<ClassViewModel> GetClassesForCopy(long classId)
        {
            var currentProfessorId = _authHelper.CurrentAccountId();

            return (from c in _context.Classes
                join t in _context.ClassTemplates on c.ClassTemplateId equals t.Id
                where t.ProfessorId == currentProfessorId && c.Id != classId
                select new ClassViewModel
                {
                    Id = c.Id,
                    Code = c.Code
                }).ToList();
        }
        public long GetTemplateIdByClassId(long classId)
        {
            return _context.Classes
                .Where(x => x.Id == classId)
                .Select(x => x.ClassTemplateId)
                .FirstOrDefault();
        }
        
        public Class GetClassByCode(string code)
        {
            return _context.Classes.FirstOrDefault(x => x.Code == code);
        }


        public ClassInfoForCopy GetClassInfoByClassCode(string classCode)
        {
            var data =
                (from c in _context.Classes
                    join t in _context.ClassTemplates on c.ClassTemplateId equals t.Id
                    join crs in _context.Courses on t.CourseId equals crs.Id
                    where c.Code == classCode
                    select new
                    {
                        c.Day,
                        c.StartTime,
                        c.EndTime,
                        CourseName = crs.Name
                    }).FirstOrDefault();

            if (data == null) return null;

            return new ClassInfoForCopy
            {
                CourseName = data.CourseName,
                Day = Days.GetName(data.Day),
                StartTime = data.StartTime,
                EndTime = data.EndTime
            };
        }

        public bool ExistsForProfessorAtTime(long professorId, int day, string startTime, long? excludeClassId = null)
        {
            var query =
                from c in _context.Classes
                join t in _context.ClassTemplates on c.ClassTemplateId equals t.Id
                where t.ProfessorId == professorId
                      && c.Day == day
                      && c.StartTime == startTime
                select c.Id;

            if (excludeClassId.HasValue && excludeClassId.Value > 0)
                query = query.Where(id => id != excludeClassId.Value);

            return query.Any();
        }

    }
}
