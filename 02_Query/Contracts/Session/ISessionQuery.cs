using StudyManagement.Application.Contracts.Order;

namespace _02_Query.Contracts.Session
{
    public interface ISessionQuery
    {
        List<CartItem> GetItemsByClassId(long classId);
        SessionQueryModel GetSessionById(long sessionId);
    }
}
