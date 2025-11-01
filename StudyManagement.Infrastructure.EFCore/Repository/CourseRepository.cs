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
                Major = x.Major,
                UniversityType = x.UniversityType,
                University = x.University,
                Price = x.Price,
                EducationLevel = x.EducationLevel
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
            if (searchModel.Major >0)
            {
                query = query.Where(x => x.Major == searchModel.Major);
            }
            if (searchModel.University > 0)
            {
                query = query.Where(x => x.University == searchModel.University);
            }
            if (searchModel.UniversityType > 0)
            {
                query = query.Where(x => x.UniversityType == searchModel.UniversityType);
            }
            if (searchModel.EducationLevel > 0)
            {
                query = query.Where(x => x.EducationLevel == searchModel.EducationLevel);
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
                Major = x.Major,
                Price = x.Price,
                UniversityType = x.UniversityType,
                University = x.University,
                EducationLevel = x.EducationLevel
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
