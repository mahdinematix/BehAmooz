using _01_Framework.Application;
using _01_Framework.Infrastructure;
using AccountManagement.Application.Contract;
using AccountManagement.Application.Contract.Account;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Pages;

public class SMSModel : UserContextPageModel
{
    private readonly IAccountApplication _accountApplication;
    private readonly IOtpApplication _otpApplication;
    public SMSModel(IAuthHelper authHelper, IAccountApplication accountApplication, IOtpApplication otpApplication) : base(authHelper)
    {
        _accountApplication = accountApplication;
        _otpApplication = otpApplication;
    }

    [TempData] public string Message { get; set; }
    [BindProperty] public string NationalCode { get; set; }
    [BindProperty] public int Type { get; set; }
    public string HiddenPhoneNumber { get; set; }
    public int RemainingSeconds { get; set; }
    public async Task<IActionResult> OnGet(string nationalCode, int type)
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

        Type = type;
        NationalCode = nationalCode;
        var phoneNumber = _accountApplication.GetPhoneNumberByNationalCode(nationalCode);
        HiddenPhoneNumber = phoneNumber.Substring(8, 3);
        try
        {
            await _otpApplication.RequestOtpAsync(phoneNumber, type);
            RemainingSeconds = await _otpApplication.GetOtpRemainingSecondsAsync(phoneNumber, type);
        }
        catch (Exception ex)
        {
            Message = ApplicationMessages.ProblemInProgress;
            return Page();
        }
        return Page();
    }

    public async Task<IActionResult> OnPost(string userOtpCode)
    {
        var phoneNumber = _accountApplication.GetPhoneNumberByNationalCode(NationalCode);
        
        bool isVerified = await _otpApplication.VerifyOtpAsync(phoneNumber, userOtpCode, Type);



        if (isVerified)
        {
            if (Type == OtpType.Login)
            {
                await _accountApplication.FinalLogin(NationalCode);
                return RedirectToPage("/Index");
            }
            else
            {
                return RedirectToPage("/ResetPassword", new { nationalCode = NationalCode });
            }
        }

        Message = ApplicationMessages.WrongOtpCode;
        return RedirectToPage("/SMS", new
        {
            nationalCode = NationalCode, Type
        });

    }
    public async Task<IActionResult> OnGetResendOtp(string nationalCode, int type)
    {
        var phoneNumber = _accountApplication.GetPhoneNumberByNationalCode(nationalCode);
        await _otpApplication.RequestOtpAsync(phoneNumber, type);
        return new JsonResult(new { success = true });
    }

}