namespace StudyManagement.Application.Contracts.Order;

public class OrderViewModel
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public string AccountFullname { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public double PayAmount { get; set; }
    public bool IsPayed { get; set; }
    public bool IsCanceled { get; set; }
    public string IssueTrackingNo { get; set; }
    public long RefId { get; set; }
    public string CreationDate { get; set; }

}