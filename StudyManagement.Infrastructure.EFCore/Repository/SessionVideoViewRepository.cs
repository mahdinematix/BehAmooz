using _01_Framework.Infrastructure;
using StudyManagement.Domain.SessionVideoViewAgg;

namespace StudyManagement.Infrastructure.EFCore.Repository
{
    public class SessionVideoViewRepository:RepositoryBase<long,SessionVideoView>,ISessionVideoViewRepository
    {
        private readonly StudyContext _studyContext;

        public SessionVideoViewRepository(StudyContext studyContext): base(studyContext)
        {
            _studyContext = studyContext;
        }

        public SessionVideoView Get(long accountId, long sessionId)
        {
            return _studyContext.SessionVideoViews
                .FirstOrDefault(x =>
                    x.AccountId == accountId &&
                    x.SessionId == sessionId);
        }
    }
}
