namespace StudyManagement.Application.Contracts.Class;

public class ClassViewModel
{
    public long Id { get; set; }
    public string Code { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public bool IsActive { get; set; }
    public long CourseId { get; set; }
    public string Course { get; set; }
    public string CreationDate { get; set; }
    public int SessionsCount { get; set; }
    public string Day { get; set; }
}