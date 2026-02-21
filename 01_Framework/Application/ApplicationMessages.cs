namespace _01_Framework.Application;

public class ApplicationMessages
{
    public const string DuplicatedRecord = "رکورد تکراری است. لطفا فیلد آن را تغییر دهید.";
    public const string DuplicatedRecordNationalCodeOrCode = "کد ملی یا کد دانشجویی/استادی وارد شده در سیستم موجود است.";
    public const string NotFoundRecord = "رکوردی با اطلاعات درخواست شده یافت نشد. لطفا دوباره امتحان کنید.";
    public const string ASessionWithThatNumberExists = "جلسه ای باهمین شماره وجود دارد. لطفا مجدد تلاش کنید.";
    public const string StartTimeAndEndTimeHaveInterference = "ساعت شروع و پایان باهم تداخل دارند!";
    public const string AClassExistsWithTheStartTime = "کلاسی با همین روز و ساعت وجود دارد!";

    public const string PasswordsNotMatch = "پسوورد و تکرار ان باهم تطابق ندارند.";
    public const string WrongPasswordOrUsername = "نام کاربری یا رمز عبور اشتباه است.";
    public const string WrongNowPassword = "رمز عبور فعلی اشتباه است.";

    public const string PaymentSuccessful = "پرداخت با موفقیت انجام شد. ممنون از خرید و اعتماد شما.";
    public const string PaymentFailed = "پرداخت با خطا مواجه شد. در صورت کسر از حساب شما، مبلغ نهایتا تا 72 ساعت کاری دیگر به حسابتان باز گردانده می شود.";

    public const string PaymentByCash =
        "سفارش شما با موفقیت ثبت شد. پس از تماس کارشناسان ما با شما، سفارش ارسال خواهد شد.";
    public const string InvalidNationalCode = "کد ملی نامعتبر است.";
    public const string RequestedAmountMoreThanYourBalance = "مقدار درخواستی بیشتر از موجودی شماست.";

    public const string UploadProgressCanceled = "فرایند آپلود با موفقیت لغو شد.";
    public const string SelectYourUniversity = "دانشگاه را انتخاب کنید...";
    public const string SelectYourUniversityType = "نوع دانشگاه را انتخاب کنید...";
    public const string FillTheForms = "نوع دانشگاه را انتخاب کنید...";


    public static string PasswordRulesAreNotPassed = "شروط کلمه عبور را رعایت نکردید!";
    public static string VideoWatched3 = "شما ۳ بار این ویدیو را مشاهده کرده‌اید.";
    public static string TheClassHasNotAnySessions = "این کلاس هیچ جلسه‌ای ندارد.";
}


