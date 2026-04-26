namespace _01_Framework.Application.Sms
{
    public interface ISmsService
    {
        Task<bool> Send(string phoneNumber, string otpCode);
    }
}