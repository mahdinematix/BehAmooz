namespace AccountManagement.Application.Contract;

public interface IOtpApplication
{
    Task RequestOtpAsync(string phone, int type);
    Task<bool> VerifyOtpAsync(string phone, string code, int type);
    Task<int> GetOtpRemainingSecondsAsync(string phoneNumber, int type);
}

