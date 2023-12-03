using System.Text.Json;
using BasketAPI.Entities;
using Microsoft.Extensions.Caching.Distributed;

namespace BasketAPI.Repositories;
public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }

    public async Task<ShoppingCart> GetBasket(string username)
    {
        var basket = await _redisCache.GetStringAsync(username);
        if (basket is null) return null;
        return JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task RemoveBasket(string username)
    {
        await _redisCache.RemoveAsync(username);
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart cart)
    {
        await _redisCache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart));
        return await GetBasket(cart.UserName);
    }
}
