using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Course;
using StudyManagement.Domain.CourseAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class CourseRepository : RepositoryBase<long,Course> , ICourseRepository
    {
        private readonly StudyContext _context;

        public CourseRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public List<CourseViewModel> Search(CourseSearchModel searchModel)
        {
            var query = _context.Courses.Include(x=>x.Classes).Select(x => new CourseViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                NumberOfUnit = x.NumberOfUnit,
                CourseKind = x.CourseKind,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate.ToFarsi(),
                ClassesCount = x.Classes.Count,
                Major = x.Major
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code == searchModel.Code);
            }
            if (searchModel.CourseKind != "0" && searchModel.CourseKind != null)
            {
                query = query.Where(x => x.CourseKind == searchModel.CourseKind);
            }
            if (searchModel.Major != "0" && searchModel.Major != null)
            {
                query = query.Where(x => x.Major == searchModel.Major);
            }

            return query.OrderByDescending(x => x.Id).ToList();
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
                Major = x.Major
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<CourseViewModel> GetCourses()
        {
            return _context.Courses.Where(x=>x.IsActive).Select(x => new CourseViewModel
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
