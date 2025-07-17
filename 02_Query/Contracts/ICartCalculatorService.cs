
using StudyManagement.Application.Contracts.Order;

namespace _02_Query.Contracts
{
    public interface ICartCalculatorService
    {
        Cart ComputeCart(List<CartItem> cartItems);
    }
}
