namespace StudyManagement.Application.Contracts.Semester
{
    public class SemesterViewModel
    {
        public long Id { get; set; }
        public string MidYear { get; set; }
        public int Year { get; set; }
        public int Code { get; set; }
        public bool IsCurrent { get; set; }
        public long UniversityId { get; set; }
        public string UniversityName { get; set; }
    }
}
