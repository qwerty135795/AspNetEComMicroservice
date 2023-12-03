using BasketAPI.Entities;

namespace BasketAPI.Repositories;
public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string username);
    Task<ShoppingCart> UpdateBasket(ShoppingCart cart);
    Task RemoveBasket(string username);
}
