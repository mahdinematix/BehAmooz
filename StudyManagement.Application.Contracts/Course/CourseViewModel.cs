namespace StudyManagement.Application.Contracts.Course;

public class CourseViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int NumberOfUnit { get; set; }
    public string Code { get; set; }
    public string CourseKind { get; set; }
    public string CreationDate { get; set; }
    public bool IsActive { get; set; }
    public long ClassesCount { get; set; }
}