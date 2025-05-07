namespace StudyManagement.Application.Contracts.SessionPicture;

public class SessionPictureViewModel
{
    public long Id { get; set; }
    public long SessionId { get; set; }
    public string Picture { get; set; }
    public string CreationDate { get; set; }
    public bool IsRemoved { get; set; }
}