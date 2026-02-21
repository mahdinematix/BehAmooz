using _01_Framework.Application;
using StudyManagement.Application.Contracts.SessionVideoView;
using StudyManagement.Domain.SessionVideoViewAgg;

namespace StudyManagement.Application
{
    public class SessionVideoViewApplication: ISessionVideoViewApplication
    {
        private readonly ISessionVideoViewRepository _repository;

        public SessionVideoViewApplication(ISessionVideoViewRepository sessionVideoViewRepository)
        {
            _repository = sessionVideoViewRepository;
        }

        public VideoWatchResult TryWatch(long sessionId,long currentAccountId)
        {
            var result = new VideoWatchResult();
            var accountId = currentAccountId;

            var view = _repository.Get(accountId, sessionId);

            if (view == null)
            {
                view = new SessionVideoView(accountId, sessionId);
                _repository.Create(view);
                _repository.Save();

                result.IsSucceeded = true;
                result.RemainingCount = 2;
                return result;
            }

            if (view.CanWatch())
            {
                view.Increase();
                _repository.Save();

                result.IsSucceeded = true;
                result.RemainingCount = 3 - view.ViewCount;
                return result;
            }

            result.IsSucceeded = false;
            result.Message = ApplicationMessages.VideoWatched3;
            result.RemainingCount = 0;
            return result;
        }
        public int GetRemainingCount(long sessionId,long currentAccountId)
        {
            var view = _repository.Get(currentAccountId, sessionId);

            if (view == null)
                return 3;

            return Math.Max(0, 3 - view.ViewCount);
        }

    }
}
