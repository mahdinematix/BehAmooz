using _02_Query.Contracts.University;
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
    }
}
