using _01_Framework.Application;

namespace StudyManagement.Application.Contracts.SessionPicture
{
    public interface ISessionPictureApplication
    {
        OperationResult Create(CreateSessionPicture command);
        OperationResult Edit(EditSessionPicture command);
        OperationResult Remove(long id);
        OperationResult Restore(long id);
        EditSessionPicture GetDetails(long id);
        List<SessionPictureViewModel> GetSessionPicturesBySessionId(long sessionId);
    }
}
