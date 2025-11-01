namespace AccountManagement.Application.Contract.Wallet
{
    public class RequestWithdrawDto
    {
        public long AccountId { get; set; }
        public long Amount { get; set; }
        public string CreditCardNo { get; set; }
    }
}
