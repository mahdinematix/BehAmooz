using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class CourseRepository : RepositoryBase<long, Course>, ICourseRepository
    {
        private readonly StudyContext _context;

        public CourseRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public List<CourseViewModel> Search(CourseSearchModel searchModel, long universityId, long currentAccountId, string currentAccountRole)
        {
            var coursesQuery = _context.Courses
                .Include(x => x.Semester)
                .Where(x => x.UniversityId == universityId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                coursesQuery = coursesQuery.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                coursesQuery = coursesQuery.Where(x => x.Code == searchModel.Code);

            if (!string.IsNullOrWhiteSpace(searchModel.CourseKind) && searchModel.CourseKind != "0")
                coursesQuery = coursesQuery.Where(x => x.CourseKind == searchModel.CourseKind);

            if (searchModel.Major > 0)
                coursesQuery = coursesQuery.Where(x => x.Major == searchModel.Major);

            if (searchModel.EducationLevel > 0)
                coursesQuery = coursesQuery.Where(x => x.EducationLevel == searchModel.EducationLevel);

            if (searchModel.SemesterId > 0)
                coursesQuery = coursesQuery.Where(x => x.SemesterId == searchModel.SemesterId);

            var courses = coursesQuery
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.NumberOfUnit,
                    x.CourseKind,
                    x.IsActive,
                    x.CreationDate,
                    x.Major,
                    x.Price,
                    x.EducationLevel,
                    SemesterCode = x.Semester.Code,
                    x.SemesterId,
                    x.UniversityId
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            var courseIds = courses.Select(x => x.Id).ToList();

            var classCountsQuery =
                from t in _context.ClassTemplates
                join c in _context.Classes on t.Id equals c.ClassTemplateId
                where courseIds.Contains(t.CourseId)
                select new
                {
                    t.CourseId,
                    t.ProfessorId,
                    ClassId = c.Id
                };

            if (currentAccountRole == Roles.Professor)
                classCountsQuery = classCountsQuery.Where(x => x.ProfessorId == currentAccountId);

            var classCounts = classCountsQuery
                .GroupBy(x => x.CourseId)
                .Select(g => new { CourseId = g.Key, Count = g.Count() })
                .ToDictionary(x => x.CourseId, x => x.Count);

            return courses.Select(x => new CourseViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                NumberOfUnit = x.NumberOfUnit,
                CourseKind = x.CourseKind,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate.ToFarsi(),
                ClassesCount = classCounts.TryGetValue(x.Id, out var cnt) ? cnt : 0,
                Major = x.Major,
                Price = x.Price,
                EducationLevel = x.EducationLevel,
                SemesterCode = x.SemesterCode,
                SemesterId = x.SemesterId,
                UniversityId = x.UniversityId
            }).ToList();
        }

        public EditCourse GetDetails(long id)
        {
            return _context.Courses.Select(x => new EditCourse
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                NumberOfUnit = x.NumberOfUnit,
                CourseKind = x.CourseKind,
                Major = x.Major,
                Price = x.Price,
                EducationLevel = x.EducationLevel,
                SemesterId = x.SemesterId,
                UniversityId = x.UniversityId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<CourseViewModel> GetCourses()
        {
            return _context.Courses.Where(x => x.IsActive).Select(x => new CourseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public CourseViewModel GetById(long courseId)
        {
            return _context.Courses.Select(x => new CourseViewModel
            {
                Id = x.Id,
                Name = x.Name,
                UniversityId = x.UniversityId
            }).FirstOrDefault(x => x.Id == courseId);
        }
    }
}
