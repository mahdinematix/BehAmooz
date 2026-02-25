namespace _01_Framework.Infrastructure
{
    public static class TargetTypes
    {
        public const int Course = 1;
        public const int Class = 2;
        public const int Session = 3;
        public const int Semester = 4;


        public static string GetTargetTypeBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "درس";
                case 2:
                    return "کلاس";
                case 3:
                    return "جلسه";
                case 4:
                    return "ترم";
                default:
                    return "";
            }
        }
    }
}
