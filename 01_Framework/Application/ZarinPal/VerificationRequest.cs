namespace _01_Framework.Application.ZarinPal
{
    public class VerificationRequest
    {
        public long amount { get; set; }
        public string merchant_id { get; set; }
        public string authority { get; set; }
    }
}
