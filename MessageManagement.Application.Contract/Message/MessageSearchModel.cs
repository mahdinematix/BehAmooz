namespace MessageManagement.Application.Contract.Message;

public class MessageSearchModel
{
    public string Title { get; set; }
    public string MessageFor { get; set; }
    public bool ForAllUniversities { get; set; }
    public long UniversityId { get; set; }
    public int UniversityTypeId { get; set; }
    
}