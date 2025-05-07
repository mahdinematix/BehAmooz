using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.Session
{
    public interface ISessionApplication
    {
        OperationResult Create(CreateSession command);
        OperationResult Edit(EditSession command);
        OperationResult Activate(long id);
        OperationResult DeActivate(long id);
        EditSession GetDetails(long id);
        List<SessionViewModel> GetAllByClassId(long classId);
        SessionViewModel GetBySessionId(long sessionId);
    }
}
