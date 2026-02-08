namespace _01_Framework.Infrastructure
{
    public static class Operations
    {
        public const int Create = 1;
        public const int Edit = 2;
        public const int Activate = 3;
        public const int Deactivate = 4;
        public const int Delete = 5;
        public const int Copy = 6;

        public static string GetOperationBy(long id)
        {
            switch (id)
            {
                case 1:
                    return "ایجاد";
                case 2:
                    return "ویرایش";
                case 3:
                    return "فعالسازی";
                case 4:
                    return "غیرفعالسازی";
                case 5:
                    return "حذف";
                case 6:
                    return "کپی";
                default:
                    return "";
            }
        }
    }
}
