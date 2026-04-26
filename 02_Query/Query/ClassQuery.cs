using _02_Query.Contracts.Class;
using _02_Query.Contracts.Course;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Infrastructure.EFCore;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class ClassQuery : IClassQuery
    {
        private readonly StudyContext _studyContext;
        private readonly AccountContext _accountContext;

        public ClassQuery(StudyContext studyContext, AccountContext accountContext)
        {
            _studyContext = studyContext;
            _accountContext = accountContext;
        }

        public List<ClassQueryModel> GetClassesByCourseId(long courseId)
        {
            var accounts = _accountContext.Accounts
                .Select(a => new { a.Id, FullName = a.FirstName + " " + a.LastName })
                .ToList();

            var classes = (
                    from c in _studyContext.Classes
                    join t in _studyContext.ClassTemplates on c.ClassTemplateId equals t.Id
                    where c.IsActive && t.CourseId == courseId
                    select new ClassQueryModel
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Day = c.Day,
                        StartTime = c.StartTime,
                        EndTime = c.EndTime,
                        IsActive = c.IsActive,
                        CourseId = t.CourseId,
                        ProfessorId = t.ProfessorId,
                        SessionsCount = _studyContext.Sessions.Count(s => s.ClassTemplateId == t.Id && s.IsActive)
                    })
                .OrderByDescending(x => x.Id)
                .ToList();

            classes.ForEach(item =>
            {
                item.ProfessorFullName = accounts.FirstOrDefault(a => a.Id == item.ProfessorId)?.FullName;
            });

            return classes;
        }

        public ClassQueryModel GetClassById(long classId)
        {
            var accounts = _accountContext.Accounts
                .Select(a => new { a.Id, FullName = a.FirstName + " " + a.LastName })
                .ToList();

            var classs = (
                    from c in _studyContext.Classes
                    join t in _studyContext.ClassTemplates on c.ClassTemplateId equals t.Id
                    where c.Id == classId
                    select new ClassQueryModel
                    {
                        Id = c.Id,
                        Code = c.Code,
                        Day = c.Day,
                        StartTime = c.StartTime,
                        EndTime = c.EndTime,
                        IsActive = c.IsActive,

                        CourseId = t.CourseId,
                        ProfessorId = t.ProfessorId,

                        SessionsCount = _studyContext.Sessions.Count(s => s.ClassTemplateId == t.Id)
                    })
                .FirstOrDefault();

            if (classs == null) return null;

            classs.ProfessorFullName = accounts.FirstOrDefault(a => a.Id == classs.ProfessorId)?.FullName;
            return classs;
        }

        
    }
}
