using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Session
{
    public interface ISessionApplication
    {
        Task<OperationResult> Create(CreateSession command);
        Task<OperationResult> Edit(EditSession command);
        OperationResult Activate(long id);
        OperationResult DeActivate(long id);
        EditSession GetDetails(long id);
        List<SessionViewModel> GetAllByClassId(long classId);
        SessionViewModel GetBySessionId(long sessionId);
    }
}
