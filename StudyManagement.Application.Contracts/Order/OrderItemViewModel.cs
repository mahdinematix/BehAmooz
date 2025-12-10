namespace StudyManagement.Application.Contracts.Order
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public int SessionNumber { get; set; }
        public string ProfessorFullName { get; set; }
        public string ClassStartTime { get; set; }
        public string ClassEndTime { get; set; }
        public int ClassDay { get; set; }
        public string CourseName { get; set; }
        public int UnitPrice { get; set; }
        public long OrderId { get; set; }
    }
}