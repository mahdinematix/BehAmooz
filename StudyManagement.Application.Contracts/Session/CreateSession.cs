namespace StudyManagement.Application.Contracts.Session
{
    public class CreateSession
    {
        public string Number { get; set; }
        public string Title { get; set; }
        public string Video { get; set; }
        public string Booklet { get; set; }
        public string Description { get; set; }
        public long ClassId { get; set; }
    }
}
