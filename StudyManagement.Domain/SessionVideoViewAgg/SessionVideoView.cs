using _01_Framework.Domain;

namespace StudyManagement.Domain.SessionVideoViewAgg
{
    public class SessionVideoView : EntityBase
    {
        public long AccountId { get; private set; }
        public long SessionId { get; private set; }
        public int ViewCount { get; private set; }

        protected SessionVideoView() { }

        public SessionVideoView(long accountId, long sessionId)
        {
            AccountId = accountId;
            SessionId = sessionId;
            ViewCount = 1;
        }

        public bool CanWatch()
        {
            return ViewCount < 3;
        }

        public void Increase()
        {
            ViewCount++;
        }
    }
}
