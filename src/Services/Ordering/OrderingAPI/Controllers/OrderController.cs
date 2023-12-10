using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderingApplication.Features.Command.CheckoutOrder;
using OrderingApplication.Features.Command.DeleteOrder;
using OrderingApplication.Features.Command.UpdateOrder;
using OrderingApplication.Features.Queries.GetOrdersList;

namespace OrderingAPI.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{username}", Name = "GetOrders")]
    [ProducesResponseType(typeof(IEnumerable<OrdersVm>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<OrdersVm>>> GetOrdersByUsername(string username)
    {
        var query = new GetOrdersListQuery(username);
        
        var orders = await _mediator.Send(query);

        return Ok(orders); 
    }
    [HttpPost(Name = "CheckoutOrder")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<ActionResult> CheckoutOrder(CheckoutOrderCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut(Name = "UpdateOrder")]
    [ProducesResponseType((int) HttpStatusCode.NoContent)]
    [ProducesResponseType((int) HttpStatusCode.NotFound)]
    [ProducesDefaultResponseType]
    public async  Task<ActionResult> UpdateOrder(UpdateOrderCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }
    [HttpDelete("{id}", Name = "DeleteOrder")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        var command = new DeleteOrderCommand { Id = id};
        await _mediator.Send(command);
        return NoContent();
    } 
}
