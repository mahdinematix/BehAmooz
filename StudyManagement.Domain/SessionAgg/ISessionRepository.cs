using _01_Framework.Domain;
using StudyManagement.Application.Contracts.Session;

namespace StudyManagement.Domain.SessionAgg
{
    public interface ISessionRepository : IRepositoryBase<long, Session>
    {
        EditSession GetDetails(long id);
        List<SessionViewModel> GetAllByClassTemplateId(long classTemplateId);
        List<Session> GetAllByClassTemplateIdForCopy(long classTemplateId);
        SessionViewModel GetBySessionId(long sessionId);

        void Delete(Session session);

        bool HasAnySessionsByClassTemplateId(long classTemplateId);

        void DeleteAllByClassTemplateId(long classTemplateId);
    }
}
