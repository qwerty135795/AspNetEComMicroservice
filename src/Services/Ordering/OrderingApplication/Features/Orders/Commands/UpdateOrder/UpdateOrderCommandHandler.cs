using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Persistence;
using OrderingApplication.Exceptions;
using OrderingDomain.Entities;

namespace OrderingApplication.Features.Command.UpdateOrder;
public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;

    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper,
    ILogger<UpdateOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);
        if (orderToUpdate is null)
        {
            throw new NotFoundException(nameof(Order), request.Id.ToString());
        }
        _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand),typeof(Order));
        await _orderRepository.UpdateAsync(orderToUpdate);

        return Unit.Value;
    }
}
