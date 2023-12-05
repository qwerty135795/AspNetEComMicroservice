using System.Net;
using DiscountAPI.Entities;
using DiscountAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DiscountAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountController(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }
    [HttpGet("{productName}", Name= "GetDiscount")]
    [ProducesResponseType(typeof(Coupon),(int)HttpStatusCode.OK)]
    public async Task<ActionResult<Coupon>> GetDiscountAsync(string productName)
    {
        var coupon = await _discountRepository.GetDiscount(productName);
        return Ok(coupon);
    }
    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<Coupon>> CreateDiscount(Coupon coupon)
    {
        var result = await _discountRepository.CreateDiscount(coupon);
        return result ? CreatedAtRoute("GetDiscount",new {productName=coupon.ProductName}, coupon) : BadRequest(); 
    }
    [HttpPut]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> UpdateDiscount(Coupon coupon)
    {
        var result = await _discountRepository.UpdateDiscount(coupon);
        return result ? Ok(coupon) : BadRequest();
    }
    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    [ProducesResponseType((int) HttpStatusCode.OK)]
    [ProducesResponseType((int) HttpStatusCode.BadRequest)]
    public async Task<ActionResult> RemoveDiscount(string productName)
    {
        var result = await _discountRepository.RemoveDiscount(productName);

        return result ? Ok() : BadRequest();
    }
}