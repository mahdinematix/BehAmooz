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

        public CourseQuery(StudyContext studyContext)
        {
            _studyContext = studyContext;
        }

        public List<CourseQueryModel> Search(CourseSearchModel searchModel, AuthViewModel currentAccountInfo)
        {
            var semesters = _studyContext.Semesters
                .Select(x => new { x.Id, x.Code, x.UniversityId, x.IsCurrent })
                .ToList();

            var currentSemester = semesters
                .Where(x => x.UniversityId == currentAccountInfo.UniversityId)
                .FirstOrDefault(x => x.IsCurrent)?.Id ?? 0;

            IQueryable<Course> courseQuery;

            if (currentAccountInfo.RoleId.ToString() == Roles.Student)
            {
                courseQuery = _studyContext.Courses
                    .Where(x => x.Major == currentAccountInfo.MajorId)
                    .Where(x => x.IsActive)
                    .Where(x => x.SemesterId == currentSemester)
                    .Where(x => x.EducationLevel == currentAccountInfo.EducationLevel)
                    .Include(x => x.University)
                    .AsQueryable();
            }
            else if (currentAccountInfo.RoleId.ToString() == Roles.Administrator)
            {
                courseQuery = _studyContext.Courses
                    .Where(x => x.IsActive)
                    .Where(x => x.SemesterId == currentSemester)
                    .Where(x => x.UniversityId == currentAccountInfo.UniversityId)
                    .Include(x => x.University)
                    .AsQueryable();
            }
            else
            {
                courseQuery = _studyContext.Courses
                    .Where(x => x.IsActive)
                    .Include(x => x.University)
                    .AsQueryable();
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                courseQuery = courseQuery.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                courseQuery = courseQuery.Where(x => x.Code == searchModel.Code);

            if (!string.IsNullOrWhiteSpace(searchModel.CourseKind) && searchModel.CourseKind != "0")
                courseQuery = courseQuery.Where(x => x.CourseKind == searchModel.CourseKind);

            if (searchModel.Major > 0)
                courseQuery = courseQuery.Where(x => x.Major == searchModel.Major);

            if (searchModel.EducationLevel > 0)
                courseQuery = courseQuery.Where(x => x.EducationLevel == searchModel.EducationLevel);

            if (searchModel.UniversityTypeId > 0)
                courseQuery = courseQuery.Where(x => x.University.Type == searchModel.UniversityTypeId);

            if (searchModel.UniversityId > 0)
                courseQuery = courseQuery.Where(x => x.UniversityId == searchModel.UniversityId);

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
                    SemesterId = x.SemesterId,
                    UniversityId = x.UniversityId,
                    UniversityTypeId = x.University.Type,
                    UniversityName = x.University.Name,
                    SemesterCode = semesters.FirstOrDefault(s => s.Id == x.SemesterId)?.Code ?? 0
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            return courses;
        }

        public CourseQueryModel GetCourseNameAndPriceById(long courseId)
        {
            return _studyContext.Courses.Where(x => x.Id == courseId).Select(x => new CourseQueryModel
            {
                Name = x.Name,
                Price = x.Price
            }).FirstOrDefault();
        }
    }
}
