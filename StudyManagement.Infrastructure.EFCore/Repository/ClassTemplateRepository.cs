using _01_Framework.Infrastructure;
using StudyManagement.Domain.ClassAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class ClassTemplateRepository : RepositoryBase<long , ClassTemplate>, IClassTemplateRepository
    {
        private readonly StudyContext _context;

        public ClassTemplateRepository(StudyContext context) : base(context) 
        {
            _context = context;
        }


        public ClassTemplate GetBy(long courseId, long professorId)
        {
            return _context.ClassTemplates
                .FirstOrDefault(x =>
                    x.CourseId == courseId &&
                    x.ProfessorId == professorId);
        }

        public long GetIdBy(long courseId, long professorId)
        {
            return _context.ClassTemplates
                .Where(x =>
                    x.CourseId == courseId &&
                    x.ProfessorId == professorId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }
    }
}
