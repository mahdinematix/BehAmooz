namespace _02_Query.Contracts.Customer
{
    public class CustomerQueryModel
    {
        public long AccountId { get; set; }
        public string FullName { get; set; }
        public string Code { get; set; }
        public int UniversityId { get; set; }
        public string CourseName { get; set; }
        public string ClassCode { get; set; }
        public int ClassDay { get; set; }
        public string ClassStartTime { get; set; }
        public string ClassEndTime { get; set; }
        public Dictionary<int, int> SessionCounts { get; set; } 
        public int TotalSessions { get; set; }
        public int SessionPrice { get; set; }
        public int TotalAmount { get; set; }
    }
}
