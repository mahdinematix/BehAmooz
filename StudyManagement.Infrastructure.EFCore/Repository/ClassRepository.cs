using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract.Account;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Class;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class ClassRepository: RepositoryBase<long,Class>, IClassRepository
    {
        private readonly StudyContext _context;
        private readonly AccountContext _accountContext; 

        public ClassRepository(StudyContext context, AccountContext accountContext) : base(context)
        {
            _context = context;
            _accountContext = accountContext;
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
                Day = x.Day,
                ProfessorId = x.ProfessorId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ClassViewModel> Search(ClassSearchModel searchModel , long courseId)
        {
            var accounts = _accountContext.Accounts.Select(x=> new AccountViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName
            });
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
                DayId = x.Day,
                Day = Days.GetName(x.Day),
                SessionsCount = x.Sessions.Count,
                ProfessorId = x.ProfessorId,
                
            });
            

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
            {
                query = query.Where(x => x.Code.Contains(searchModel.Code));
            }

            if (searchModel.StartTime != "0" && searchModel.StartTime != null)
            {
                query = query.Where(x => x.StartTime == searchModel.StartTime);
            }
            if (searchModel.DayId >0)
            {
                query = query.Where(x => x.DayId == searchModel.DayId);
            }

            if (searchModel.IsActive)
            {
                query = query.Where(x => !x.IsActive);
            }

            var classes = query.OrderByDescending(x => x.Id).ToList();

            classes.ForEach(item =>
            {
                item.ProfessorFullName = accounts.FirstOrDefault(x => x.Id == item.ProfessorId)?.FullName;
            });
            return classes;
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
                Day = Days.GetName(x.Day),
                DayId = x.Day,
                StartTime = x.StartTime,
                Course = x.Course.Name,
                CourseId = x.Course.Id,
                SessionsCount = x.Sessions.Count
            }).FirstOrDefault(x => x.Id == id);
        }
    }
}
