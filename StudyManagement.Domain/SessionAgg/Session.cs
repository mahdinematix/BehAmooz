using _01_Framework.Domain;
using StudyManagement.Domain.ClassAgg;
using StudyManagement.Domain.SessionPictureAgg;

namespace StudyManagement.Domain.SessionAgg
{
    public class Session : EntityBase
    {
        public int Number { get; private set; }
        public string Title { get; private set; }
        public string Video { get; private set; }
        public string Booklet { get; private set; }
        public string Description { get; private set; }
        public long ClassId { get; private set; }
        public bool IsActive { get; private set; }
        public Class Class { get; private set; }
        public ICollection<SessionPicture> SessionPictures { get; private set; }

        public Session(int number, string title, string video, string booklet, string description, long classId)
        {
            Number = number;
            Title = title;
            if (!string.IsNullOrWhiteSpace(video))
            {
                Video = video;
            }
            else
            {
                Video = "";
            }
            if (!string.IsNullOrWhiteSpace(booklet))
            {
                Booklet = booklet;
            }
            else
            {
                Booklet = "";
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = description;
            }
            else
            {
                Description = "";
            }
            ClassId = classId;
            IsActive = true;
            SessionPictures = new List<SessionPicture>();
        }

        public void Edit(int number, string title, string video, string booklet, string description, long classId)
        {
            Number = number;
            Title = title;
            if (!string.IsNullOrWhiteSpace(video))
            {
                Video = video;
            }
            if (!string.IsNullOrWhiteSpace(booklet))
            {
                Booklet = booklet;
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = description;
            }
            
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
