using _02_Query.Contracts.SessionPicture;

namespace _02_Query.Contracts.Session
{
    public class SessionQueryModel
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Video { get; set; }
        public string Booklet { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public long ClassId { get; set; }
        public bool IsActive { get; set; }
        public bool IsPayed { get; set; }
        public List<SessionPictureQueryModel> SessionPictures { get; set; }
    }
}
