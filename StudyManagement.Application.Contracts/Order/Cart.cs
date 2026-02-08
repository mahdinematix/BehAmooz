namespace StudyManagement.Application.Contracts.Order;

public class Cart
{
    public int Tax { get; set; }
    public int TotalAmount { get; set; }
    public int FinalAmount { get; set; }
    

    public List<CartItem> Items { get; set; }

    public Cart()
    {
        Items = new List<CartItem>();
    }

    public void Add(CartItem cartItem)
    {
        Items.Add(cartItem);
        TotalAmount += cartItem.SessionPrice;
        Tax = TotalAmount * 10 / 100;
        FinalAmount = TotalAmount + Tax;
    }

}