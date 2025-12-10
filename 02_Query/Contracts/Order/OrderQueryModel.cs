namespace _02_Query.Contracts.Order
{
    public class OrderQueryModel
    {
        public long Id { get; set; }
        public string PayDate { get; set; }
        public bool IsPaid { get; set; }
        public int Amount { get; set; }
        public string IssueTrackingNo { get; set; }
    }
}
