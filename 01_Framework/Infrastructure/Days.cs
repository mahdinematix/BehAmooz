using System.Security.Cryptography.X509Certificates;

namespace _01_Framework.Infrastructure
{
    public class Days
    {
        public static List<DayViewModel> List = new List<DayViewModel>
        {
            new DayViewModel("شنبه"),
            new DayViewModel("یکشنبه"),
            new DayViewModel("دوشنبه"),
            new DayViewModel("سه‌شنبه"),
            new DayViewModel("چهارشنبه"),
            new DayViewModel("پنجشنبه"),
            new DayViewModel("جمعه")
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
