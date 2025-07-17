using StudyManagement.Application.Contracts.Order;

namespace StudyManagement.Application
{
    public class CartService : ICartService
    {
        public Cart Cart { get; set; }
        public Cart Get()
        {
            return Cart;
        }

        public void Set(Cart cart)
        {
            Cart = cart;
        }
    }
}
