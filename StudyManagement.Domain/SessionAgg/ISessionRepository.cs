using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Session;

namespace StudyManagement.Domain.SessionAgg
{
    public interface ISessionRepository : IRepositoryBase<long, Session>
    {
        EditSession GetDetails(long id);
        List<SessionViewModel> GetAllByClassId(long classId);
        SessionViewModel GetBySessionId(long sessionId);
    }
}
