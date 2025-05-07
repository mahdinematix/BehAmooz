using _01_Framework.Domain;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.SessionPictureAgg;

namespace StudyManagement.Domain.SessionAgg
{
    public class Session : EntityBase
    {
        public string Number { get; private set; }
        public string Title { get; private set; }
        public string Video { get; private set; }
        public string Booklet { get; private set; }
        public string Description { get; private set; }
        public long ClassId { get; private set; }
        public bool IsActive { get; private set; }
        public Class Class { get; private set; }
        public ICollection<SessionPicture> SessionPictures { get; private set; }

        public Session(string number, string title, string video, string booklet, string description, long classId)
        {
            Number = number;
            Title = title;
            Video = video;
            Booklet = booklet;
            Description = description;
            ClassId = classId;
            IsActive = true;
            SessionPictures = new List<SessionPicture>();
        }

        public void Edit(string number, string title, string video, string booklet, string description, long classId)
        {
            Number = number;
            Title = title;
            Video = video;
            Booklet = booklet;
            Description = description;
            ClassId = classId;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void DeActivate()
        {
            IsActive = false;
        }
    }

}
