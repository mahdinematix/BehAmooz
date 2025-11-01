namespace StudyManagement.Application.Contracts.Course;

public class CourseSearchModel
{
    public string Name { get; set; }
    public string CourseKind { get; set; }
    public string Code { get; set; }
    public int Major { get; set; }
    public int UniversityType { get; set; }
    public int University { get; set; }
    public int EducationLevel { get; set; }

}