namespace StudyManagement.Application.Contracts.Order
{
    public class OrderItemViewModel
    {
        public long Id { get; set; }
        public long SessionId { get; set; }
        public string Session { get; set; }
        public double UnitPrice { get; set; }
        public long OrderId { get; set; }
    }
}