using _01_Framework.Application;
using _02_Query.Contracts.Course;
using StudyManagement.Domain.CourseAgg;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class CourseQuery : ICourseQuery
    {
        private readonly StudyContext _studyContext;
        private readonly IAuthHelper _authHelper;

        public CourseQuery(StudyContext studyContext, IAuthHelper authHelper)
        {
            _studyContext = studyContext;
            _authHelper = authHelper;
        }

        public List<CourseQueryModel> GetCourses()
        {
            return _studyContext.Courses.Where(x => x.IsActive).Where(x=>x.Major == _authHelper.GetAccountInfo().MajorId).Where(x=>x.University == _authHelper.GetAccountInfo().UniversityId).Select(x => new CourseQueryModel
            {
                University = x.University,
                Major = x.Major,
                Code = x.Code,
                IsActive = x.IsActive,
                CourseKind = x.CourseKind,
                Name = x.Name,
                NumberOfUnit = x.NumberOfUnit,
                Id = x.Id

            }).ToList();
        }

        public string GetCourseNameById(long courseId)
        {
            return _studyContext.Courses.FirstOrDefault(x => x.Id == courseId).Name;
        }

        
    }
}
