namespace AccountManagement.Application.Contract;

public interface IOtpApplication
{
    Task RequestOtpAsync(string phone, int type);
    Task<bool> VerifyOtpAsync(string phone, string code, int type);
    Task<int> GetOtpRemainingSecondsAsync(string phoneNumber, int type);
    Task<string?> CreateAndStoreResetTokenAsync(string nationalCode);
    Task<bool> ValidateResetTokenAsync(string nationalCode, string token);
    Task RemoveResetTokenAsync(string nationalCode);
}

