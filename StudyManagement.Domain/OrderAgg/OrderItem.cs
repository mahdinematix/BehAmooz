using _01_Framework.Domain;

namespace StudyManagement.Domain.OrderAgg;

public class OrderItem : EntityBase
{
    public long SessionId { get; private set; }
    public int SessionPrice { get; private set; }
    public int SessionNumber { get; private set; }
    public long ProfessorId { get; private set; }
    public string ProfessorFullName { get; private set; }
    public string ClassStartTime { get; private set; }
    public string ClassEndTime { get; private set; }
    public int ClassDay { get; private set; }
    public long ClassId { get; private set; }
    public string CourseName { get; private set; }
    public long OrderId { get; private set; }
    public Order Order { get; private set; }


    public OrderItem(long sessionId, int sessionPrice, int sessionNumber,long professorId, string professorFullName, string classStartTime, string classEndTime, int classDay, long classId, string courseName)
    {
        SessionId = sessionId;
        SessionPrice = sessionPrice;
        SessionNumber = sessionNumber;
        ProfessorId = professorId;
        ProfessorFullName = professorFullName;
        ClassStartTime = classStartTime;
        ClassEndTime = classEndTime;
        ClassDay = classDay;
        ClassId = classId;
        CourseName = courseName;
    }
}