namespace AccountManagement.Application.Contract.Wallet
{
    public class CancelRequestWithdrawByUserDto
    {
        public long AccountId { get; set; }
        public long TransactionId { get; set; }
    }
}
