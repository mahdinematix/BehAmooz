namespace AccountManagement.Application.Contract.Wallet
{
    public class SettlementRequestSearchModel
    {
        public string FullName { get; set; }
        public long RoleId { get; set; }
        public string NationalCode { get; set; }
        public int Status { get; set; }
    }
}
