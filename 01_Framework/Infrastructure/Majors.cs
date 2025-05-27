namespace _01_Framework.Infrastructure
{
    public class Majors
    {
        public static List<MajorViewModel> List = new List<MajorViewModel>
            {
            new MajorViewModel ("اتاق عمل" ),
            new MajorViewModel ("اخلاق و تربیت اسلامی" ),
            new MajorViewModel ("ادیان و عرفان" ),
            new MajorViewModel ("ارتوپدی" ),
            new MajorViewModel ("اطفال" ),
            new MajorViewModel ("اقتصاد" ),
            new MajorViewModel ("الهیات و معارف اسلامی" ),
            new MajorViewModel ("اورولوژی" ),
            new MajorViewModel ("آموزش معلمان" ),
            new MajorViewModel ("باستان شناسی" ),
            new MajorViewModel ("برنامه ریزی آموزشی" ),
            new MajorViewModel ("بهداشت" ),
            new MajorViewModel ("بیهوشی" ),
            new MajorViewModel ("پاتوبیولوژی" ),
            new MajorViewModel ("پاتولوژی" ),
            new MajorViewModel ("پرستاری" ),
            new MajorViewModel ("پزشکی عمومی" ),
            new MajorViewModel ("پزشکی قانونی" ),
            new MajorViewModel ("پوست و زیبایی" ),
            new MajorViewModel ("تاریخ" ),
            new MajorViewModel ("تربیت بدنی" ),
            new MajorViewModel ("جراحی عمومی" ),
            new MajorViewModel ("جغرافیا" ),
            new MajorViewModel ("چشم پزشکی" ),
            new MajorViewModel ("حسابداری" ),
            new MajorViewModel ("حقوق" ),
            new MajorViewModel ("حمل و نقل" ),
            new MajorViewModel ("داخلی" ),
            new MajorViewModel ("داروسازی" ),
            new MajorViewModel ("دندانپزشکی" ),
            new MajorViewModel ("رادیولوژی" ),
            new MajorViewModel ("رباتیک" ),
            new MajorViewModel ("روانپزشکی" ),
            new MajorViewModel ("ریاضی" ),
            new MajorViewModel ("زبان انگلیسی" ),
            new MajorViewModel ("زبان و ادبیات عرب" ),
            new MajorViewModel ("زبان و ادبیات فارسی" ),
            new MajorViewModel ("زمین شناسی" ),
            new MajorViewModel ("زیست شناسی" ),
            new MajorViewModel ("ژنتیک" ),
            new MajorViewModel ("شهرسازی" ),
            new MajorViewModel ("شیمی" ),
            new MajorViewModel ("صنایع چوب و کاغذ" ),
            new MajorViewModel ("صنایع غذایی" ),
            new MajorViewModel ("علم اطلاعات و دانش شناسی" ),
            new MajorViewModel ("علوم اجتماعی" ),
            new MajorViewModel ("علوم ارتباطات" ),
            new MajorViewModel ("علوم تربیتی" ),
            new MajorViewModel ("علوم دامی و شیلات" ),
            new MajorViewModel ("علوم سیاسی و روابط بین الملل" ),
            new MajorViewModel ("علوم قران و حدیث" ),
            new MajorViewModel ("فلسفه و حکمت" ),
            new MajorViewModel ("فن آوری اطلاعات" ),
            new MajorViewModel ("فیزیک" ),
            new MajorViewModel ("فیزیولوژی" ),
            new MajorViewModel ("قارچ شناسی" ),
            new MajorViewModel ("قلب و عروق" ),
            new MajorViewModel ("کتابداری" ),
            new MajorViewModel ("گرافیک و نقاشی" ),
            new MajorViewModel ("گیاه پزشکی" ),
            new MajorViewModel ("مامایی" ),
            new MajorViewModel ("محیط زیست" ),
            new MajorViewModel ("مدیریت و بازرگانی" ),
            new MajorViewModel ("مشاوره و روانشناسی" ),
            new MajorViewModel ("معماری" ),
            new MajorViewModel ("مغز و اعصاب" ),
            new MajorViewModel ("مهندسی برق" ),
            new MajorViewModel ("مهندسی پزشکی" ),
            new MajorViewModel ("مهندسی پلیمر" ),
            new MajorViewModel ("مهندسی صنایع" ),
            new MajorViewModel ("مهندسی عمران" ),
            new MajorViewModel ("مهندسی کامپیوتر" ),
            new MajorViewModel ("مهندسی کشاورزی" ),
            new MajorViewModel ("مهندسی متالولورژی و مواد" ),
            new MajorViewModel ("مهندسی مکانیک" ),
            new MajorViewModel ("مهندسی نفت و معدن" ),
            new MajorViewModel ("مهندسی هسته ای" ),
            new MajorViewModel ("مهندسی هوا فضا" ),
            new MajorViewModel ("موسیقی" ),
            new MajorViewModel ("میکروبیولوژی" ),
            new MajorViewModel ("نورولوژی" ),
            new MajorViewModel ("هنر" ),
            new MajorViewModel ("هنرهای نمایشی" )
        };

        public static string GetName(int id)
        {
            foreach (var i in List)
            {
                if (i.Id == id)
                {
                    return i.Name;
                }
            }

            return null;
        }
    }
}
