using System.Net;
using BasketAPI.Entities;
using BasketAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
    {
        _basketRepository = basketRepository;
        _logger = logger;
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
        return Ok(await _basketRepository.UpdateBasket(cart));
    }
    [HttpDelete("{username}", Name = "RemoveBasket")]
    public async Task<IActionResult> RemoveBasket(string username)
    {
        await _basketRepository.RemoveBasket(username);
        return Ok();
    }
}