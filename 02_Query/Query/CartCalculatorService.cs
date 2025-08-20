using _02_Query.Contracts;
using StudyManagement.Application.Contracts.Order;

namespace _02_Query.Query;

public class CartCalculatorService : ICartCalculatorService
{
    public Cart ComputeCart(List<CartItem> cartItems)
    {
        var cart = new Cart();

        foreach (var cartItem in cartItems)
        {
            cart.Add(cartItem);
        }

        return cart;
    }
}