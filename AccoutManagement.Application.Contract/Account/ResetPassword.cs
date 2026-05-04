namespace AccountManagement.Application.Contract.Account
{
    public class ResetPassword
    {
        public string NationalCode { get; set; }
        public string OtpToken { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
    }
}
