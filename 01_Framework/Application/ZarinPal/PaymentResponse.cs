namespace _01_Framework.Application.ZarinPal
{
    public class PaymentResponse
    {
        public PaymentData data { get; set; }
        public List<string> errors { get; set; }
    }
}
