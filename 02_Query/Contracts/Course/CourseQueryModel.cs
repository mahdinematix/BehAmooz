namespace _02_Query.Contracts.Course
{
    public class CourseQueryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int NumberOfUnit { get; set; }
        public string CourseKind { get; set; }
        public string Code { get; set; }
        public int Major { get; set; }
        public int University { get; set; }
        public int Price { get; set; }
        public bool IsActive { get; set; }
        public int EducationLevel { get; set; }
    }
}
