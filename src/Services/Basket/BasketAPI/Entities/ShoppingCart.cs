namespace BasketAPI.Entities;

public class ShoppingCart
{
    public string UserName { get; set; }
    public List<ShoppingCartItem> Items { get; set; } = new();
    public ShoppingCart(string username)
    {
        UserName = username;
    }
    public ShoppingCart()
    {
        
    }
    public decimal TotalPrice()
    {
        return Items.Sum(l => l.Price);
    }
}