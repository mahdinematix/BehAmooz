using _01_Framework.Application;
using _01_Framework.Infrastructure;
using StudyManagement.Application.Contracts.SessionPicture;
using StudyManagement.Domain.SessionPictureAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class SessionPictureRepository : RepositoryBase<long,SessionPicture>, ISessionPictureRepository
    {
        private readonly StudyContext _context;
        public SessionPictureRepository(StudyContext context) : base(context)
        {
            _context = context;
        }

        public EditSessionPicture GetDetails(long id)
        {
            return _context.SessionPictures.Select(x => new EditSessionPicture
            {
                Id = x.Id,
                SessionId = x.SessionId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<SessionPictureViewModel> GetSessionPicturesBySessionId(long sessionId)
        {
            return _context.SessionPictures.Where(x => x.SessionId == sessionId)
                .Select(x => new SessionPictureViewModel
                {
                    Id = x.Id,
                    Picture = x.Picture,
                    SessionId = x.SessionId,
                    CreationDate = x.CreationDate.ToFarsi(),
                    IsRemoved = x.IsRemoved
                }).ToList();
        }
    }
}
