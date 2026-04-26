using _01_Framework.Application;
using _01_Framework.Application.Sms;
using AccountManagement.Application.Contract;

namespace AccountManagement.Application
{
    public class OtpApplication : IOtpApplication
    {
        private readonly IOtpStore _otpStore;
        private readonly ISmsService _smsService;
        public OtpApplication(IOtpStore otpStore, ISmsService smsService)
        {
            _otpStore = otpStore;
            _smsService = smsService;
        }

        public async Task RequestOtpAsync(string phone, int type)
        {
            var otpCode = GenerateOtpCode();

            await _otpStore.StoreAsync(phone, type, otpCode);

            await _smsService.Send(phone, otpCode);
        }

        public async Task<bool> VerifyOtpAsync(string phone, string code, int type)
        {
            var storedCode = await _otpStore.GetAsync(phone, type);

            if (storedCode == null)
                return false;

            if (storedCode != code)
                return false;

            return true;
        }
        public async Task<int> GetOtpRemainingSecondsAsync(string phoneNumber, int type)
        {
            return await _otpStore.GetOtpRemainingSecondsAsync(phoneNumber, type);
        }


        private string GenerateOtpCode()
        {
            Random random = new Random();
            string otpCode = string.Empty;

            otpCode += random.Next(1, 10).ToString();

            for (int i = 1; i < 6; i++)
            {
                otpCode += random.Next(0, 10).ToString();
            }

            return otpCode;
        }
    }
}
