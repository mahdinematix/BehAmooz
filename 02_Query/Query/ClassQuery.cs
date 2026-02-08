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
            var accounts = _accountContext.Accounts.Select(x => new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName
            });
            var classes =  _studyContext.Classes.Where(x => x.IsActive).Where(x=>x.CourseId == courseId).Select(x => new ClassQueryModel
            {
                Id = x.Id,
                Code = x.Code,
                CourseId = x.CourseId,
                Day = x.Day,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                IsActive = x.IsActive,
                ProfessorId = x.ProfessorId,
                SessionsCount = x.Sessions.Count(x=>x.IsActive),
            }).OrderByDescending(x=>x.Id).ToList();

            classes.ForEach(item =>
            {
                item.ProfessorFullName = accounts.FirstOrDefault(x => x.Id == item.ProfessorId)?.FullName;
            });
            return classes;
        }

        public ClassQueryModel GetClassById(long classId)
        {
            var accounts = _accountContext.Accounts.Select(x => new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName
            });
            var classs = _studyContext.Classes.Select(x => new ClassQueryModel
            {
                Id = x.Id,
                Code = x.Code,
                CourseId = x.CourseId,
                Day = x.Day,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                IsActive = x.IsActive,
                ProfessorId = x.ProfessorId,
                SessionsCount = x.Sessions.Count
            }).FirstOrDefault(x => x.Id == classId);

                classs.ProfessorFullName = accounts.FirstOrDefault(x => x.Id == classs.ProfessorId)?.FullName;
            return classs;
        }

        public CourseQueryModel GetCourseNameAndPriceByClassId(long courseId)
        {
            return _studyContext.Courses.Where(x=>x.Id==courseId).Select(x=> new CourseQueryModel
            {
                Name = x.Name,
                Price = x.Price
            }).FirstOrDefault();
        }
    }
}
