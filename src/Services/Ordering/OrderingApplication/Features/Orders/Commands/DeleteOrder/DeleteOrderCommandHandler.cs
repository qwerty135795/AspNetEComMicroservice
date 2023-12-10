using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Persistence;
using OrderingApplication.Exceptions;
using OrderingDomain.Entities;

namespace OrderingApplication.Features.Command.DeleteOrder;
public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrderRepository orderRepository,
    ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if(order is null)
        {
            throw new NotFoundException(nameof(Order), request.Id.ToString());
        }
        await _orderRepository.DeleteAsync(order);
        return Unit.Value;
    }
}
