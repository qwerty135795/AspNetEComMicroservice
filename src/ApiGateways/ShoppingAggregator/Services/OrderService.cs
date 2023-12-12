using ShoppingAggregator.Extensions;
using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services;
public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<OrderResponseModel>> GetOrders(string username)
    {
        var response = await _httpClient.GetAsync($"/api/v1/Order/{username}");
        return await response.ReadContentAs<IEnumerable<OrderResponseModel>>();
    }
}
