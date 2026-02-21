using System.ComponentModel;

namespace StudyManagement.Application.Contracts.Course;

public class CourseViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int NumberOfUnit { get; set; }
    public string Code { get; set; }
    public int Major { get; set; }
    public string CourseKind { get; set; }
    public int Price { get; set; }
    public string CreationDate { get; set; }
    public bool IsActive { get; set; }
    public long ClassesCount { get; set; }
    public int EducationLevel { get; set; }
    public long SemesterId { get; set; }
    public int SemesterCode { get; set; }
    public long UniversityId { get; set; }
    public string UniversityName { get; set; }

}