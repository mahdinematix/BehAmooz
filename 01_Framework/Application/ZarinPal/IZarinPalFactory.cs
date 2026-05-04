namespace _01_Framework.Application.ZarinPal
{
    public interface IZarinPalFactory
    {
        string Prefix { get; set; }

        PaymentData CreatePaymentRequest(string amount, string mobile, string email, string description,
            long orderId, int type);

        VerificationData CreateVerificationRequest(string authority, string price);
    }
}