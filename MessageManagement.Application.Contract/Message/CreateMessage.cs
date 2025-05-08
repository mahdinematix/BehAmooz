namespace MessageManagement.Application.Contract.Message
{
    public class CreateMessage
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string MessageFor { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
