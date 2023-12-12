using ShoppingAggregator.Models;

namespace ShoppingAggregator.Services;
public interface IOrderService
{
    Task<IEnumerable<OrderResponseModel>> GetOrders(string username);
}
