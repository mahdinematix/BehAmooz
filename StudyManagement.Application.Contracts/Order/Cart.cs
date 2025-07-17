namespace StudyManagement.Application.Contracts.Order;

public class Cart
{
    public double TotalAmount { get; set; }
    public int PaymentMethod { get; set; }

    public List<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
    }

    public void SetPaymentMethod(int methodId)
    {
        PaymentMethod = methodId;
    }
}