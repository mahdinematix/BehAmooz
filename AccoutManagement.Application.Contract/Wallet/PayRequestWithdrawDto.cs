namespace AccountManagement.Application.Contract.Wallet
{
    public class PayRequestWithdrawDto
    {
        public long AccountId { get; set; }
        public long TransactionId { get; set; }
        public string Description { get; set; }
    }
}
