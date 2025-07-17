using _02_Query.Contracts.Class;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
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
                SessionsCount = x.Sessions.Count,
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

        public string GetCourseNameByClassId(long classId)
        {
            return _studyContext.Classes.Include(x=>x.Course).FirstOrDefault(x => x.Id == classId).Course.Name;
        }
    }
}
