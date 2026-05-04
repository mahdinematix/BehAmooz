using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages
{
    public class ResetPasswordModel : UserContextPageModel
    {
        private readonly IAccountApplication _accountApplication;
        private readonly IOtpApplication _otpApplication;
        [TempData] public string Message { get; set; }
        [BindProperty] public ResetPassword Command { get; set; }
        public ResetPasswordModel(IAccountApplication accountApplication, IAuthHelper authHelper, IOtpApplication otpApplication) : base(authHelper)
        {
            _accountApplication = accountApplication;
            _otpApplication = otpApplication;
        }

        public async Task<IActionResult> OnGet(string nationalCode)
        {
            if (IsAuthenticated)
            {
                if (CurrentAccountRole == Roles.Student)
                {
                    return RedirectToPage("/Index");
                }

                if (CurrentAccountRole == Roles.Administrator || CurrentAccountRole == Roles.SuperAdministrator)
                {
                    return RedirectToPage("/Index", new { area = "Administration" });
                }

                if (CurrentAccountRole == Roles.Professor)
                {
                    return RedirectToPage("/Index", new { area = "Professor" });
                }

                if (CurrentAccountStatus == Statuses.Waiting)
                {
                    return RedirectToPage("/NotConfirmed");
                }

                if (CurrentAccountStatus == Statuses.Rejected)
                {
                    return RedirectToPage("/Reject");
                }
            }

            var resetToken = await _otpApplication.CreateAndStoreResetTokenAsync(nationalCode);
            if (string.IsNullOrEmpty(resetToken))
            {
                Message = ApplicationMessages.ErrorWhenCreateSecurityToken;
                return RedirectToPage("/SMS", new { nationalCode, type = OtpType.ResetPassword });
            }
            var isValidToken = await _otpApplication.ValidateResetTokenAsync(nationalCode, resetToken);

            if (!isValidToken)
            {
                return RedirectToPage("/ForgetPassword");
            }
            Command = new ResetPassword { NationalCode = nationalCode, OtpToken = resetToken };
            return Page();
        }

        public async Task<IActionResult> OnPost(ResetPassword command)
        {
            var result = _accountApplication.ResetPassword(Command);

            if (result.IsSucceeded)
            {
                Message = ApplicationMessages.PasswordResetSuccess;
                await _otpApplication.RemoveResetTokenAsync(Command.NationalCode);
                return RedirectToPage("/Login");
            }

            Message = result.Message;
            return Page();
        }
    }
}
