using _02_Query.Contracts.University;
using StudyManagement.Application.Contracts.University;
using StudyManagement.Infrastructure.EFCore;

namespace _02_Query.Query
{
    public class UniversityQuery : IUniversityQuery
    {
        private readonly StudyContext _studyContext;

        public UniversityQuery(StudyContext studyContext)
        {
            _studyContext = studyContext;
        }

        public UniversityQueryModel GetById(long id)
        {
            return _studyContext.Universities.Select(x=> new UniversityQueryModel
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                SemesterId = x.CurrentSemesterId,
                SemesterCode = x.Semesters
                    .Where(s => s.Id == x.CurrentSemesterId)
                    .Select(s => s.Code)
                    .FirstOrDefault()

            }).FirstOrDefault(x => x.Id == id);
        }

        public List<UniversityViewModel> GetActiveUniversitiesByTypeId(int typeId)
        {
            return _studyContext.Universities.Where(x => x.IsActive).Where(x => x.Type == typeId).Select(x => new UniversityViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }
    }
}
