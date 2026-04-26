using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace _01_Framework.Application.Sms
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> Send(string phoneNumber, string otpCode)
        {
            HttpClient httpClient = new HttpClient();
            var apiKey = _configuration["SmsIr:ApiKey"];
            var templateId = _configuration["SmsIr:TemplateId"];
            httpClient.DefaultRequestHeaders.Add("x-api-key",
                apiKey);
            VerifySendModel model = new VerifySendModel()
            {
                Mobile = phoneNumber,
                TemplateId = templateId,
                Parameters = new VerifySendParameterModel[]
                    {new VerifySendParameterModel {Name = "CODE", Value = otpCode}}
            };
            string payload = JsonSerializer.Serialize(model);
            StringContent stringContent = new(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response =
                await httpClient.PostAsync("https://api.sms.ir/v1/send/verify", stringContent);
            var content = response.Content.ReadAsStringAsync();
            bool result = response.IsSuccessStatusCode;
            return result;
        }
    }
}