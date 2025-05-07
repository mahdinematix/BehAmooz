using _01_Framework.Application;
using _01_Framework.Infrastructure;
using StudyManagement.Application.Contracts.Session;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class SessionRepository : RepositoryBase<long, Session> , ISessionRepository
    {
        private readonly StudyContext _context;

        public SessionRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public EditSession GetDetails(long id)
        {
            return _context.Sessions.Select(x => new EditSession
            {
                Id = x.Id,
                Booklet = x.Booklet,
                ClassId = x.ClassId,
                Description = x.Description,
                Number = x.Number,
                Title = x.Title,
                Video = x.Video
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<SessionViewModel> GetAllByClassId(long classId)
        {
            return _context.Sessions.Where(x=>x.ClassId == classId).Select(x => new SessionViewModel
            {
                Id = x.Id,
                Booklet = x.Booklet,
                Description = x.Description.Substring(0, Math.Min(x.Description.Length, 10)) + "...",
                Number = x.Number,
                Title = x.Title,
                Video = x.Video,
                IsActive = x.IsActive,
                ClassId = x.ClassId,
                CreationDate = x.CreationDate.ToFarsi()

            }).OrderByDescending(x => x.Id).ToList();
        }

        public SessionViewModel GetBySessionId(long sessionId)
        {
            return _context.Sessions.Select(x => new SessionViewModel
            {
                Id = x.Id,
                Number = x.Number,
                Title = x.Title,
                ClassId = x.ClassId
            }).FirstOrDefault(x => x.Id == sessionId);
        }
    }
}
