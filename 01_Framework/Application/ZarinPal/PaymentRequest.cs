namespace _01_Framework.Application.ZarinPal
{
    public class PaymentRequest
    {
        public string mobile { get; set; }
        public string email { get; set; }
        public string callback_url { get; set; }
        public string description { get; set; }
        public long amount { get; set; }
        public string merchant_id { get; set; }
        public string currency { get; set; }
        public string order_id { get; set; }
    }
}
