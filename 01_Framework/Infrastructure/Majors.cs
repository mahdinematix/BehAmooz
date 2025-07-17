namespace _01_Framework.Infrastructure
{
    public class Majors
    {
        public static List<MajorViewModel> List = new List<MajorViewModel>
            {
            new MajorViewModel (1,"اتاق عمل" ),
            new MajorViewModel (2,"اخلاق و تربیت اسلامی" ),
            new MajorViewModel (3,"ادیان و عرفان" ),
            new MajorViewModel (4,"ارتوپدی" ),
            new MajorViewModel (5,"اطفال" ),
            new MajorViewModel (6,"اقتصاد" ),
            new MajorViewModel (7,"الهیات و معارف اسلامی" ),
            new MajorViewModel (8,"اورولوژی" ),
            new MajorViewModel (9,"آموزش معلمان" ),
            new MajorViewModel (10,"باستان شناسی" ),
            new MajorViewModel (11,"برنامه ریزی آموزشی" ),
            new MajorViewModel (12,"بهداشت" ),
            new MajorViewModel (13,"بیهوشی" ),
            new MajorViewModel (14,"پاتوبیولوژی" ),
            new MajorViewModel (15,"پاتولوژی" ),
            new MajorViewModel (16,"پرستاری" ),
            new MajorViewModel (17,"پزشکی عمومی" ),
            new MajorViewModel (18,"پزشکی قانونی" ),
            new MajorViewModel (19,"پوست و زیبایی" ),
            new MajorViewModel (20,"تاریخ" ),
            new MajorViewModel (21,"تربیت بدنی" ),
            new MajorViewModel (22,"جراحی عمومی" ),
            new MajorViewModel (23,"جغرافیا" ),
            new MajorViewModel (24,"چشم پزشکی" ),
            new MajorViewModel (25,"حسابداری" ),
            new MajorViewModel (26,"حقوق" ),
            new MajorViewModel (27,"حمل و نقل" ),
            new MajorViewModel (28,"داخلی" ),
            new MajorViewModel (29,"داروسازی" ),
            new MajorViewModel (30,"دندانپزشکی" ),
            new MajorViewModel (31,"رادیولوژی" ),
            new MajorViewModel (32,"رباتیک" ),
            new MajorViewModel (33,"روانپزشکی" ),
            new MajorViewModel (34,"ریاضی" ),
            new MajorViewModel (35,"زبان انگلیسی" ),
            new MajorViewModel (36,"زبان و ادبیات عرب" ),
            new MajorViewModel (37,"زبان و ادبیات فارسی" ),
            new MajorViewModel (38,"زمین شناسی" ),
            new MajorViewModel (39,"زیست شناسی" ),
            new MajorViewModel (40,"ژنتیک" ),
            new MajorViewModel (41,"شهرسازی" ),
            new MajorViewModel (42,"شیمی" ),
            new MajorViewModel (43,"صنایع چوب و کاغذ" ),
            new MajorViewModel (44,"صنایع غذایی" ),
            new MajorViewModel (45,"علم اطلاعات و دانش شناسی" ),
            new MajorViewModel (46,"علوم اجتماعی" ),
            new MajorViewModel (47,"علوم ارتباطات" ),
            new MajorViewModel (48,"علوم تربیتی" ),
            new MajorViewModel (49,"علوم دامی و شیلات" ),
            new MajorViewModel (50,"علوم سیاسی و روابط بین الملل" ),
            new MajorViewModel (51,"علوم قران و حدیث" ),
            new MajorViewModel (52,"فلسفه و حکمت" ),
            new MajorViewModel (53,"فن آوری اطلاعات" ),
            new MajorViewModel (54,"فیزیک" ),
            new MajorViewModel (55,"فیزیولوژی" ),
            new MajorViewModel (56,"قارچ شناسی" ),
            new MajorViewModel (57,"قلب و عروق" ),
            new MajorViewModel (58,"کتابداری" ),
            new MajorViewModel (59,"گرافیک و نقاشی" ),
            new MajorViewModel (60,"گیاه پزشکی" ),
            new MajorViewModel (61,"مامایی" ),
            new MajorViewModel (62,"محیط زیست" ),
            new MajorViewModel (63,"مدیریت و بازرگانی" ),
            new MajorViewModel (64,"مشاوره و روانشناسی" ),
            new MajorViewModel (65,"معماری" ),
            new MajorViewModel (66,"مغز و اعصاب" ),
            new MajorViewModel (67,"مهندسی برق" ),
            new MajorViewModel (68,"مهندسی پزشکی" ),
            new MajorViewModel (69,"مهندسی پلیمر" ),
            new MajorViewModel (70,"مهندسی صنایع" ),
            new MajorViewModel (71,"مهندسی عمران" ),
            new MajorViewModel (72,"مهندسی کامپیوتر" ),
            new MajorViewModel (73,"مهندسی کشاورزی" ),
            new MajorViewModel (74,"مهندسی متالولورژی و مواد" ),
            new MajorViewModel (75,"مهندسی مکانیک" ),
            new MajorViewModel (76,"مهندسی نفت و معدن" ),
            new MajorViewModel (77,"مهندسی هسته ای" ),
            new MajorViewModel (78,"مهندسی هوا فضا" ),
            new MajorViewModel (79,"موسیقی" ),
            new MajorViewModel (80,"میکروبیولوژی" ),
            new MajorViewModel (81,"نورولوژی" ),
            new MajorViewModel (82,"هنر" ),
            new MajorViewModel (83,"هنرهای نمایشی" )
        };

        public static string GetName(int id)
        {
            var major = "";
            foreach (var i in List)
            {
                if (i.Id == id)
                {
                    major = i.Name;
                }
            }

            return major;
        }
    }
}
