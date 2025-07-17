namespace _01_Framework.Infrastructure
{
    public class UniversityTypes
    {
        public static List<UniversityTypeViewModel> List = new List<UniversityTypeViewModel>
        {
            new UniversityTypeViewModel(1,"آموزشکده سما"),
            new UniversityTypeViewModel(2,"دانشگاه آزاد اسلامی"),
            new UniversityTypeViewModel(3,"دانشگاه پیام نور"),
            new UniversityTypeViewModel(4,"دانشگاه جامع علمی کاربردی"),
            new UniversityTypeViewModel(5,"دانشگاه دولتی"),
            new UniversityTypeViewModel(6,"دانشگاه علوم پزشکی (دولتی)"),
            new UniversityTypeViewModel(7,"دانشگاه غیر انتفاعی"),
            new UniversityTypeViewModel(8,"دانشگاه فرهنگیان"),
            new UniversityTypeViewModel(9,"دانشگاه فنی و حرفه ای")
        };
        public static string GetName(int id)
        {
            var name = "";
            foreach (var i in List)
            {
                if (i.Id == id)
                {
                    name = i.Name;
                }
            }

            return name;
        }
    }
}
