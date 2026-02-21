namespace _01_Framework.Infrastructure
{
    public static class Roles
    {
        public const string Administrator = "1";
        public const string Professor = "2";
        public const string Student = "3";
        public const string SuperAdministrator = "4";

        public static string GetRoleBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "مدیر سیستم";
                case 2:
                    return "استاد";
                case 3:
                    return "دانشجو";
                case 4:
                    return "سوپر مدیر سیستم";
                default:
                    return "";
            }
        }
    }
}
