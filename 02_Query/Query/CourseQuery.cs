using _01_Framework.Application;
using _01_Framework.Infrastructure;
using _02_Query.Contracts.Course;
using Microsoft.EntityFrameworkCore;
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

        public List<CourseQueryModel> Search(CourseSearchModel searchModel)
        {
            var semesters = _studyContext.Semesters
        .Select(x => new { x.Id, x.Code, x.IsCurrent })
        .ToList();
            var currentSemester = semesters.FirstOrDefault(x => x.IsCurrent).Id;
            IQueryable<Course> courseQuery;
            if (_authHelper.CurrentAccountRole()==Roles.Student)
            {
                courseQuery = _studyContext.Courses.Where(x=>x.Major==_authHelper.GetAccountInfo().MajorId).Where(x=>x.IsActive).Where(x=>x.SemesterId==currentSemester).Where(x=>x.EducationLevel==_authHelper.CurrentAccountEducationLevel())
                    .Include(x => x.Classes)
                    .AsQueryable();
            }
            else
            {
                courseQuery = _studyContext.Courses.Where(x => x.IsActive).Where(x => x.SemesterId == currentSemester)
                    .Include(x => x.Classes)
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                courseQuery = courseQuery.Where(x=>x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                courseQuery = courseQuery.Where(x => x.Code == searchModel.Code);

            if (searchModel.CourseKind != "0" && searchModel.CourseKind != null)
                courseQuery = courseQuery.Where(x => x.CourseKind == searchModel.CourseKind);


            var courses = courseQuery
                .AsEnumerable()
                .Select(x => new CourseQueryModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    NumberOfUnit = x.NumberOfUnit,
                    CourseKind = x.CourseKind,
                    IsActive = x.IsActive,
                    Major = x.Major,
                    Price = x.Price,
                    EducationLevel = x.EducationLevel,
                    SemesterCode = semesters.FirstOrDefault(s=>s.Id ==x.SemesterId).Code
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            return courses;
        }

        public string GetCourseNameById(long courseId)
        {
            return _studyContext.Courses.FirstOrDefault(x => x.Id == courseId).Name;
        }
    }
}
