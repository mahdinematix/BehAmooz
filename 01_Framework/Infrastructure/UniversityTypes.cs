namespace _01_Framework.Infrastructure
{
    public class UniversityTypes
    {
        public static List<UniversityTypeViewModel> List = new List<UniversityTypeViewModel>
        {
            new UniversityTypeViewModel("آموزشکده سما"),
            new UniversityTypeViewModel("دانشگاه آزاد اسلامی"),
            new UniversityTypeViewModel("دانشگاه پیام نور"),
            new UniversityTypeViewModel("دانشگاه جامع علمی کاربردی"),
            new UniversityTypeViewModel("دانشگاه دولتی"),
            new UniversityTypeViewModel("دانشگاه علوم پزشکی (دولتی)"),
            new UniversityTypeViewModel("دانشگاه غیر انتفاعی"),
            new UniversityTypeViewModel("دانشگاه فرهنگیان"),
            new UniversityTypeViewModel("دانشگاه فنی و حرفه ای")
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
