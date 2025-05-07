using _01_Framework.Application;
using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class ClassRepository: RepositoryBase<long,Class>, IClassRepository
    {
        private readonly StudyContext _context;

        public ClassRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public EditClass GetDetails(long id)
        {
            return _context.Classes.Select(x => new EditClass
            {
                Id = x.Id,
                Code = x.Code,
                CourseId = x.CourseId,
                EndTime = x.EndTime.ToShownTime(),
                StartTime = x.StartTime.ToShownTime(),
                Day = x.Day
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ClassViewModel> Search(ClassSearchModel searchModel , long courseId)
        {
            var query = _context.Classes.Where(x=>x.CourseId == courseId).Include(x=>x.Course).Include(x=>x.Sessions).Select(x => new ClassViewModel
            {
                Id = x.Id,
                Code = x.Code,
                CourseId = x.CourseId,
                EndTime = x.EndTime.ToString(),
                StartTime = x.StartTime.ToString(),
                IsActive = x.IsActive && x.Course.IsActive,
                Course = x.Course.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                Day = x.Day,
                SessionsCount = x.Sessions.Count
            });
            

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.StartTime))
            {
                query = query.Where(x => x.StartTime.Contains(searchModel.StartTime));
            }

            if (searchModel.IsActive)
            {
                query = query.Where(x => !x.IsActive);
            }

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public List<ClassViewModel> GetClasses()
        {
            return _context.Classes.Select(x => new ClassViewModel
            {
                Id = x.Id,
                Code = x.Code
            }).ToList();
        }

        public ClassViewModel GetClassById(long id)
        {
            return _context.Classes.Include(x=>x.Course).Include(x=>x.Sessions).Select(x => new ClassViewModel
            {
                Id = x.Id,
                Code = x.Code,
                Day = x.Day,
                StartTime = x.StartTime,
                Course = x.Course.Name,
                CourseId = x.Course.Id,
                SessionsCount = x.Sessions.Count
            }).FirstOrDefault(x => x.Id == id);
        }
    }
}
