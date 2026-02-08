namespace AccountManagement.Application.Contract.Wallet
{
    public class BuyFromWalletDto
    {
        public long AccountId { get; set; }
        public long Amount { get; set; }
        public long OrderId { get; set; }
    }
}
