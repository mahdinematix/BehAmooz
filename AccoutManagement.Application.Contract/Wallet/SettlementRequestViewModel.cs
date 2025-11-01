namespace AccountManagement.Application.Contract.Wallet;
public class SettlementRequestViewModel
{
    public long TransactionId { get; set; }
    public string FullName { get; set; }
    public string NationalCode { get; set; }
    public long RoleId { get; set; }
    public string CreationDate { get; set; }
    public string CreditCardNo { get; set; }
    public long Amount { get; set; }
    public int Status { get; set; }
    public int Type { get; set; }
    public string Description { get; set; }
}

