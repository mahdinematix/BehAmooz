using _01_Framework.Domain;

namespace MessageManagement.Domain.MessageAgg
{
    public class Message : EntityBase
    {
        public string Title { get; private set; }
        public string Body { get; private set; }
        public string MessageFor { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }


        public Message(string title, string body,string messageFor, DateTime startDate, DateTime endDate)
        {
            Title = title;
            Body = body;
            MessageFor = messageFor;
            StartDate = startDate;
            EndDate = endDate;
        }
        public void Edit(string title, string body, string messageFor, DateTime startDate, DateTime endDate)
        {
            Title = title;
            Body = body;
            MessageFor = messageFor;
            StartDate = startDate;
            EndDate = endDate;
        }

    }
}
