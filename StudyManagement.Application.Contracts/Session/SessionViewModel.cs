namespace StudyManagement.Application.Contracts.Session;

public class SessionViewModel
{
    public long Id { get; set; }
    public string Number { get; set; }
    public string Title { get; set; }
    public string Video { get; set; }
    public string Booklet { get; set; }
    public string Description { get; set; }
    public long ClassId { get; set; }
    public bool IsActive { get; set; }
    public string CreationDate { get; set; }

}