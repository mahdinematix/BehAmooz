namespace _02_Query.Contracts.Order
{
    public interface IOrderQuery
    {
        List<OrderItemQueryModel> GetItemsThatPaid();
        bool IsPaid(long sessionId);
        List<OrderQueryModel> GetOrdersThatPaid();
    }
}
