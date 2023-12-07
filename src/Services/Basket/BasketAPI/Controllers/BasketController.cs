using System.Net;
using BasketAPI.Entities;
using BasketAPI.Repositories;
using GrpcServices;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketController> _logger;
    private readonly DiscountGrpcService _discountGrpc;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger,
    DiscountGrpcService discountGrpc)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _discountGrpc = discountGrpc;
    }
    [HttpGet("{username}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetShoppingCart(string username)
    {
        return Ok(await _basketRepository.GetBasket(username) ?? new ShoppingCart(username));
    }
    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket(ShoppingCart cart) 
    {
        foreach(var item in cart.Items)
        {
            var discount =  await _discountGrpc.GetDiscount(item.ProductName);
            item.Price -= discount.Amount;
        }
        return Ok(await _basketRepository.UpdateBasket(cart));
    }
    [HttpDelete("{username}", Name = "RemoveBasket")]
    public async Task<IActionResult> RemoveBasket(string username)
    {
        await _basketRepository.RemoveBasket(username);
        return Ok();
    }
}