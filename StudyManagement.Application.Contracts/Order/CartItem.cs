namespace StudyManagement.Application.Contracts.Order
{
    public class CartItem
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public int ClassDay { get; set; }
        public string ClassStartTime { get; set; }
        public string ClassEndTime { get; set; }
        public string CourseName { get; set; }
        public string ProfessorFullName { get; set; }
        public int SessionNumber { get; set; }
        public int SessionPrice { get; set; }
        public long SessionId { get; set; }
        public long ProfessorId { get; set; }
    }
}
