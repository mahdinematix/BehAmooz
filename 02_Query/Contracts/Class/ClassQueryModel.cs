namespace _02_Query.Contracts.Class
{
    public class ClassQueryModel
    {
        public long Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Day { get; set; }
        public string Code { get; set; }
        public long ProfessorId { get; set; }
        public long CourseId { get; set; }
        public bool IsActive { get; set; }
        public int SessionsCount { get; set; }
        public string ProfessorFullName { get; set; }
    }
}
