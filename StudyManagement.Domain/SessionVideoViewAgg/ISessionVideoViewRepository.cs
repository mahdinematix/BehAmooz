using _01_Framework.Domain;

namespace StudyManagement.Domain.SessionVideoViewAgg
{
    public interface ISessionVideoViewRepository : IRepositoryBase<long, SessionVideoView>
    {
        SessionVideoView Get(long accountId, long sessionId);
    }
}
