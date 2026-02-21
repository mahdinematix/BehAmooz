using _01_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudyManagement.Application.Contracts.Semester;
using StudyManagement.Domain.SemesterAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class SemesterRepository : RepositoryBase<long , Semester>, ISemesterRepository
    {
        private readonly StudyContext _context;
        public SemesterRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public Semester GetCurrent()
        {
            return _context.Semesters.FirstOrDefault(x => x.IsCurrent);
        }

        public SemesterViewModel GetCurrentSemester()
        {
            return _context.Semesters.Include(x=>x.University).Select(x=> new SemesterViewModel
            {
                Id = x.Id,
                Code = x.Code,
                IsCurrent = x.IsCurrent,
                MidYear = MidYears.GetMidYearBy(x.MidYear),
                Year = x.Year,
                UniversityId = x.UniversityId,
                UniversityName = x.University.Name
            }).FirstOrDefault(x => x.IsCurrent);
        }

        public Semester GetByYearAndMidYear(int year, int midYear, long universityId)
        {
            return _context.Semesters
                .FirstOrDefault(x => x.Year == year && x.MidYear == midYear && x.UniversityId ==universityId);
        }

        public List<SemesterViewModel> GetSemesters()
        {
            return _context.Semesters.Select(x => new SemesterViewModel
            {
                Id = x.Id,
                Code = x.Code
            }).ToList();
        }

        public int GetSemesterCodeBy(long semesterId)
        {
            return _context.Semesters.FirstOrDefault(x => x.Id == semesterId).Code;
        }
    }
}
