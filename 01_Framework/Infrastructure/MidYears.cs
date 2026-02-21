namespace _01_Framework.Infrastructure
{
    public static class MidYears
    {
        public const int First = 1;
        public const int Second = 2;
        public const int Summer = 3;


        public static string GetMidYearBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "اول";
                case 2:
                    return "دوم";
                case 3:
                    return "تابستان";
                default:
                    return "";
            }
        }
    }
}
