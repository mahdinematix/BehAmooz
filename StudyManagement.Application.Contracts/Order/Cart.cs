namespace StudyManagement.Application.Contracts.Order;

public class Cart
{
    public int TotalAmount { get; set; }

    public List<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
        TotalAmount += cartItem.SessionPrice;
    }

}