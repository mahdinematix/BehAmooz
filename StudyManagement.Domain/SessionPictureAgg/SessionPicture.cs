using _01_Framework.Domain;
using StudyManagement.Domain.SessionAgg;

namespace StudyManagement.Domain.SessionPictureAgg
{
    public class SessionPicture : EntityBase
    {
        public long SessionId { get; private set; }
        public string Picture { get; private set; }
        public bool IsRemoved { get; private set; }
        public Session Session { get; private set; }

        public SessionPicture(long sessionId, string picture)
        {
            SessionId = sessionId;
            Picture = picture;
            IsRemoved = false;
        }

        public void Edit(long sessionId, string picture)
        {
            SessionId = sessionId;
            Picture = picture;
        }

        public void Remove()
        {
            IsRemoved = true;
        }

        public void Restore()
        {
            IsRemoved = false;
        }
    }
}
