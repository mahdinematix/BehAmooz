namespace _02_Query.Contracts.Order
{
    public class OrderItemQueryModel
    {
        public long SessionId { get; set; }
        public int SessionNumber { get; set; }
        public int SessionPrice { get; set; }
        public string ProfessorFullName { get; set; }
        public string ClassStartTime { get; set; }
        public string ClassEndTime { get; set; }
        public int ClassDay { get; set; }
        public string CourseName { get; set; }
        public long OrderId { get; set; }
        public string PayDate { get; set; }
        public bool IsPaid { get; set; }
    }
}
