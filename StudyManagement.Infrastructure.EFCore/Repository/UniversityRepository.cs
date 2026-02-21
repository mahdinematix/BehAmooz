using _01_Framework.Application;
using _01_Framework.Infrastructure;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Domain.UniversityAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class UniversityRepository : RepositoryBase<long, University>, IUniversityRepository
    {
        private readonly StudyContext _context;

        public UniversityRepository(StudyContext context): base(context)
        {
            _context = context;
        }

        public List<UniversityViewModel> Search(UniversitySearchModel searchModel)
        {
            var query = _context.Universities.Select(x => new UniversityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CurrentSemesterId = x.CurrentSemesterId,
                CurrentSemesterCode = x.Semesters
                    .Where(s => s.Id == x.CurrentSemesterId)
                    .Select(s => s.Code)
                    .FirstOrDefault()
                ,
                Type = x.Type,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.Type>0)
            {
                query = query.Where(x => x.Type == searchModel.Type);
            }
            if (searchModel.CurrentSemesterId > 0)
            {
                query = query.Where(x => x.CurrentSemesterId == searchModel.CurrentSemesterId);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {
                query = query.Where(x => x.Name.Contains(searchModel.Name));
            }
            
            return query.OrderByDescending(x => x.Id).ToList();
        }

        public EditUniversity GetDetails(long id)
        {
            return _context.Universities.Select(x => new EditUniversity
            {
                Id = x.Id,
                Type = x.Type,
                Name = x.Name
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<UniversityViewModel> GetUniversitiesByType(int type)
        {
            return _context.Universities.Where(x=>x.Type ==type).Select(x => new UniversityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                CurrentSemesterId = x.CurrentSemesterId,
                Type = x.Type,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate.ToFarsi()
            }).ToList();
        }

        public long GetCurrentSemesterId(long id)
        {
            return _context.Universities.FirstOrDefault(x => x.Id == id).CurrentSemesterId;
        }

        public string GetNameBy(long id)
        {
            return _context.Universities.FirstOrDefault(x => x.Id == id).Name;
        }

        public List<UniversityViewModel> GetActiveUniversities()
        {
            return _context.Universities.Where(x => x.IsActive).Select(x => new UniversityViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
        public List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId)
        {
            return _context.Universities.Where(x => x.IsActive).Where(x=>x.Type==typeId).Select(x => new UniversityViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
