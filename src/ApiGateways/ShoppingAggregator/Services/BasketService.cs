using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services;
public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;

    public BasketService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BasketModel> GetBasket(string username)
    {
        var response = await _httpClient.GetAsync($"/api/v1/Basket/{username}");
        return await response.ReadContentAs<BasketModel>(); 
    }
}
