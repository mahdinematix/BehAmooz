namespace _02_Query.Contracts.SessionPicture
{
    public class SessionPictureQueryModel
    {
        public long SessionId { get; set; }
        public string Picture { get; set; }
        public bool IsRemoved { get; set; }
    }
}
