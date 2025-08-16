using _01_Framework.Domain;

namespace StudyManagement.Domain.OrderAgg;

public class OrderItem : EntityBase
{
    public long SessionId { get; private set; }
    public int SessionPrice { get; private set; }
    public string SessionNumber { get; private set; }
    public string ProfessorFullName { get; private set; }
    public string ClassStartTime { get; private set; }
    public string ClassEndTime { get; private set; }
    public int ClassDay { get; private set; }
    public string CourseName { get; private set; }
    public long OrderId { get; private set; }
    public Order Order { get; private set; }


    public OrderItem(long sessionId, int sessionPrice, string sessionNumber, string professorFullName, string classStartTime, string classEndTime, int classDay, string courseName)
    {
        SessionId = sessionId;
        SessionPrice = sessionPrice;
        SessionNumber = sessionNumber;
        ProfessorFullName = professorFullName;
        ClassStartTime = classStartTime;
        ClassEndTime = classEndTime;
        ClassDay = classDay;
        CourseName = courseName;
    }
}