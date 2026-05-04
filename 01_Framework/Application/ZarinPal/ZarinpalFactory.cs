using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Serialization.Json;


namespace _01_Framework.Application.ZarinPal
{
    public class ZarinPalFactory : IZarinPalFactory
    {
        private readonly IConfiguration _configuration;

        public string Prefix { get; set; }
        private string MerchantId { get;}

        public ZarinPalFactory(IConfiguration configuration)
        {
            _configuration = configuration;
            Prefix = _configuration.GetSection("Payment")["method"];
            MerchantId= _configuration.GetSection("Payment")["merchant"];
        }

        public PaymentData CreatePaymentRequest(string amount, string mobile, string email, string description,
             long orderId, int type)
        {
            amount = amount.Replace(",", "");
            var finalAmount = long.Parse(amount);
            var siteUrl = _configuration.GetSection("payment")["siteUrl"];
            var pageModel = PaymentTypes.GetType(type);
            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/v4/payment/request.json");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = new PaymentRequest
            {
                mobile = mobile,
                callback_url = $"{siteUrl}/{pageModel}?handler=CallBack&oId={orderId}&amount={finalAmount}",
                description = description,
                email = email,
                amount = finalAmount,
                merchant_id = MerchantId,
                order_id = orderId.ToString(),
                currency = "IRT"
            };
            
            request.AddJsonBody(body);
            var response = client.Execute(request);
            var jsonSerializer = new JsonDeserializer();
            var paymentResponse = jsonSerializer.Deserialize<PaymentResponse>(response);
            var authority = paymentResponse.data.authority;
            var status = response.ResponseStatus.ToString();


            return new PaymentData
            {
                authority = authority,
                Status = status
            };
            
        }

        public VerificationData CreateVerificationRequest(string authority, string amount)
        {
            var client = new RestClient($"https://{Prefix}.zarinpal.com/pg/v4/payment/verify.json");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            amount = amount.Replace(",", "");
            var finalAmount = long.Parse(amount);

            request.AddJsonBody(new VerificationRequest
            {
                amount = finalAmount,
                merchant_id= MerchantId,
                authority = authority
            });
            var response = client.Execute(request);
            var jsonSerializer = new JsonDeserializer();
            var verificationResponse = jsonSerializer.Deserialize<VerificationResponse>(response);
            var code = verificationResponse.data.code;
            var refId = verificationResponse.data.ref_id;


            return new VerificationData
            {
                code = code,
                ref_id = refId
            };
        }
    }
}