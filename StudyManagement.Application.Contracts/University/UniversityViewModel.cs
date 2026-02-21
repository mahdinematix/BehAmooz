namespace StudyManagement.Application.Contracts.University;

public class UniversityViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public long CurrentSemesterId { get; set; }
    public int CurrentSemesterCode { get; set; }
    public bool IsActive { get; set; }
    public string CreationDate { get; set; }
}