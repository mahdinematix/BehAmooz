namespace _01_Framework.Application
{
    public static class EducationLevels
    {
        public const int Kardani = 1;
        public const int Karshenasi = 2;
        public const int Arshad = 3;
        public const int Doktora = 4;
        public const int FoghDoktora = 5;
        public const int ProfessorOrAdmin = 6;

        public static string GetEducationLevelById(int id)
        {
            switch (id)
            {
                case 1:
                    return "کاردانی";
                case 2:
                    return "کارشناسی";
                case 3:
                    return "ارشد";
                case 4:
                    return "دکتری";
                case 5:
                    return "فوق دکتری";
                case 6:
                    return "استاد هستم";
                default:
                    return "";
            }
        }
    }
}
