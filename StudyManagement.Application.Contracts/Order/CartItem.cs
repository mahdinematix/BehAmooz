namespace StudyManagement.Application.Contracts.Order
{
    public class CartItem
    {
        public long Id { get; set; }
        public int ClassDay { get; set; }
        public string ClassStartTime { get; set; }
        public string ClassEndTime { get; set; }
        public string CourseName { get; set; }
        public string ProfessorFullName { get; set; }
        public string SessionNumber { get; set; }
        public double SessionPrice { get; set; }
        public long SessionId { get; set; }
    }
}
