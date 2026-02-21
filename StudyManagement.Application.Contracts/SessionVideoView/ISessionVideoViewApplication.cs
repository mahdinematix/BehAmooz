namespace StudyManagement.Application.Contracts.SessionVideoView
{
    public interface ISessionVideoViewApplication
    {
        VideoWatchResult TryWatch(long sessionId, long currentAccountId);
        public int GetRemainingCount(long sessionId,long currentAccountId);
    }
}
