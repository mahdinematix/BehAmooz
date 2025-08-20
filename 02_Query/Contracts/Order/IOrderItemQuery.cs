namespace _02_Query.Contracts.Order
{
    public interface IOrderItemQuery
    {
        List<OrderItemQueryModel> GetItemsThatPaid();
        bool IsPaid(long sessionId);
    }
}
