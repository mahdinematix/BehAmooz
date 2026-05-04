namespace _01_Framework.Application
{
    public interface IOtpStore
    {   
        Task StoreAsync(string phone, int type, string code);
        Task<string?> GetAsync(string phone, int type);
        Task<int> GetOtpRemainingSecondsAsync(string phoneNumber, int type);
        Task StoreTokenAsync(string key, string token);
        Task<string?> GetTokenAsync(string key);
        Task RemoveTokenAsync(string key);
    }
}
