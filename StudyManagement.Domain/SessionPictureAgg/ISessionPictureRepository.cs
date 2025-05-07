using _01_Framework.Domain;
using StudyManagement.Application.Contracts.SessionPicture;

namespace StudyManagement.Domain.SessionPictureAgg
{
    public interface ISessionPictureRepository : IRepositoryBase<long,SessionPicture>
    {
        EditSessionPicture GetDetails(long id);
        List<SessionPictureViewModel> GetSessionPicturesBySessionId(long sessionId);
    }
}
