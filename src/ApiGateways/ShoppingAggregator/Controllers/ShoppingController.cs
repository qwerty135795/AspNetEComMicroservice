using Microsoft.AspNetCore.Mvc;
using ShoppingAggregator.Models;
using ShoppingAggregator.Services;

namespace ShoppingAggregator.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingController : ControllerBase
{
    private readonly ICatalogService _catalog;
    private readonly IBasketService _basket;
    private readonly IOrderService _order;

    public ShoppingController(ICatalogService catalog, IBasketService basket, IOrderService order)
    {
        _catalog = catalog;
        _basket = basket;
        _order = order;
    }
    [HttpGet("{username}")]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string username)
    {
        var basket = await _basket.GetBasket(username);
        foreach(var item in basket.Items)
        {
            var catalogItem = await _catalog.GetCatalog(item.ProductId);
            item.Summary = catalogItem.Summary;
            item.ProductName = catalogItem.Name;
            item.Category = catalogItem.Category;
            item.Description = catalogItem.Description;
            item.ImageFile = catalogItem.ImageFile;
        }
        var orders = await _order.GetOrders(username);

        return new ShoppingModel() {
            UserName = username, BasketWithProducts = basket,
            Orders = orders
        };
    }
}
