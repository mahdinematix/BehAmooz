namespace MessageManagement.Application.Contract.Message;

public class MessageViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string MessageFor { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public DateTime StartDateGr { get; set; }
    public DateTime EndDateGr { get; set; }
    public string CreationDate { get; set; }
}