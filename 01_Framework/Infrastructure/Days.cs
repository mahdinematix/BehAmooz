using System.Security.Cryptography.X509Certificates;

namespace _01_Framework.Infrastructure
{
    public class Days
    {
        public static List<DayViewModel> List = new List<DayViewModel>
        {
            new DayViewModel(1,"شنبه"),
            new DayViewModel(2,"یکشنبه"),
            new DayViewModel(3,"دوشنبه"),
            new DayViewModel(4,"سه‌شنبه"),
            new DayViewModel(5,"چهارشنبه"),
            new DayViewModel(6,"پنجشنبه"),
            new DayViewModel(7,"جمعه")
        };

        public static string GetName(int id)
        {
            var result = "";
            foreach (var item in List)
            {
                if (item.Id == id)
                {
                    result = item.Name;
                }
            }

            return result;
        }
    }
}
