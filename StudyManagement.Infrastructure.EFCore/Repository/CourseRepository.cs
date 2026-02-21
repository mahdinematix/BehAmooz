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
                    .Include(x => x.Classes).Include(x => x.Semester).Where(x => x.UniversityId == universityId)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                coursesQuery = coursesQuery.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                coursesQuery = coursesQuery.Where(x => x.Code == searchModel.Code);

            if (searchModel.CourseKind != "0" && searchModel.CourseKind != null)
                coursesQuery = coursesQuery.Where(x => x.CourseKind == searchModel.CourseKind);

            if (searchModel.Major > 0)
                coursesQuery = coursesQuery.Where(x => x.Major == searchModel.Major);

            if (searchModel.EducationLevel > 0)
                coursesQuery = coursesQuery.Where(x => x.EducationLevel == searchModel.EducationLevel);

            if (searchModel.SemesterId > 0)
                coursesQuery = coursesQuery.Where(x => x.SemesterId == searchModel.SemesterId);

            var courses = coursesQuery
                .AsEnumerable()
                .Select(x => new CourseViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    NumberOfUnit = x.NumberOfUnit,
                    CourseKind = x.CourseKind,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate.ToFarsi(),
                    ClassesCount = currentAccountRole == Roles.Professor
                        ? x.Classes.Count(c => c.ProfessorId == currentAccountId)
                        : x.Classes.Count,
                    Major = x.Major,
                    Price = x.Price,
                    EducationLevel = x.EducationLevel,
                    SemesterCode = x.Semester.Code,
                    SemesterId = x.SemesterId,
                    UniversityId = x.UniversityId,
                    UniversityName = x.University.Name
                })
                .OrderByDescending(x => x.Id)
                .ToList();

            return courses;
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

        public CourseViewModel GetByCourseId(long courseId)
        {
            return _context.Courses.Select(x => new CourseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).FirstOrDefault(x => x.Id == courseId);
        }
    }
}
