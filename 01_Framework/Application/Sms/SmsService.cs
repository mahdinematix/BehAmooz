using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SmsIrRestful;

namespace _01_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Send(string number, string message)
        {
            var token = GetToken();
            var lines = new SmsLine().GetSmsLines(token);
            if (lines == null) return;

            var line = lines.SMSLines.Last().LineNumber.ToString();
            var data = new MessageSendObject
            {
                Messages = new List<string>
                    {message}.ToArray(),
                MobileNumbers = new List<string> { number }.ToArray(),
                LineNumber = line,
                SendDateTime = DateTime.Now,
                CanContinueInCaseOfError = true
            };
            var messageSendResponseObject =
                new MessageSend().Send(token, data);

            if (messageSendResponseObject.IsSuccessful) return;

            line = lines.SMSLines.First().LineNumber.ToString();
            data.LineNumber = line;
            new MessageSend().Send(token, data);
        }

        private string GetToken()
        {
            var smsSecrets = _configuration.GetSection("SmsSecrets");
            var tokenService = new Token();
            return tokenService.GetToken(smsSecrets["ApiKey"], smsSecrets["secretKey"]);
        }

        //private async Task<string> GetTokenn()
        //{
        //    HttpClient httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("x-api-key",
        //        "5AjUpQILp9t7D2UdaoaJxxxxJdXX0c1dAo456usriKbgyYXqblciFvTm5NLM2346Ipcs");
        //    VerifySendModel model = new VerifySendModel()
        //    {
        //        Mobile = "9120000000",
        //        TemplateId = 123456,
        //        Parameters = new VerifySendParameterModel[]
        //            {new VerifySendParameterModel {Name = "CODE", Value = "1234"}}
        //    };
        //    string payload = JsonSerializer.Serialize(model);
        //    StringContent stringContent = new(payload, Encoding.UTF8, "application/json");
        //    HttpResponseMessage response =
        //        await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", stringContent);
        //}


        //public class VerifySendParameterModel 
        //{ 
        //    public string Name { get; set; }
        //    public string Value { get; set; }

        //}
        //public class VerifySendModel
        //{
        //    public string Mobile { get; set; }
        //    public int TemplateId { get; set; }
        //    public VerifySendParameterModel[] Parameters { get; set; }

        //}
    
}
}