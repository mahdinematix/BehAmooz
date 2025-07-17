using _01_Framework.Application;
using _02_Query.Contracts;
using StudyManagement.Application.Contracts.Order;

namespace _02_Query.Query;

public class CartCalculatorService : ICartCalculatorService
{
    private readonly IAuthHelper _authHelper;

    public CartCalculatorService(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    public Cart ComputeCart(List<CartItem> cartItems)
    {
        var cart = new Cart();
        var currentAccountRole = _authHelper.CurrentAccountRole();

        foreach (var cartItem in cartItems)
        {
            cart.Add(cartItem);
        }

        return cart;
    }
}