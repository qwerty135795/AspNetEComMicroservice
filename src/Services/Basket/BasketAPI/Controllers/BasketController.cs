using System.Net;
using AutoMapper;
using BasketAPI.Entities;
using BasketAPI.Repositories;
using EventBusMessages.Events;
using GrpcServices;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace BasketAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketController> _logger;
    private readonly DiscountGrpcService _discountGrpc;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger,
    DiscountGrpcService discountGrpc, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _basketRepository = basketRepository;
        _logger = logger;
        _discountGrpc = discountGrpc;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
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
    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult> CheckoutBasket(BasketCheckout basketCheckout)
    {
        var bask = await _basketRepository.GetBasket(basketCheckout.UserName);
        if (bask is null) return BadRequest();
        basketCheckout.TotalPrice = bask.TotalPrice();
        await _publishEndpoint.Publish(_mapper.Map<BasketCheckoutEvent>(basketCheckout));
        await _basketRepository.RemoveBasket(basketCheckout.UserName);
        return Accepted();
    }
    [HttpDelete("{username}", Name = "RemoveBasket")]
    public async Task<IActionResult> RemoveBasket(string username)
    {
        await _basketRepository.RemoveBasket(username);
        return Ok();
    }
}