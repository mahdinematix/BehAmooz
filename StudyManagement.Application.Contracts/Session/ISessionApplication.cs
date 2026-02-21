using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Session
{
    public interface ISessionApplication
    {
        Task<OperationResult> Create(CreateSession command, long currentAccountId);
        Task<OperationResult> Edit(EditSession command, long currentAccountId);
        OperationResult Activate(long id, long currentAccountId);
        OperationResult DeActivate(long id, long currentAccountId);
        EditSession GetDetails(long id);
        List<SessionViewModel> GetAllByClassId(long classId);
        SessionViewModel GetBySessionId(long sessionId);
        bool HasAnySessionsByClassId(long classId);
    }
}
