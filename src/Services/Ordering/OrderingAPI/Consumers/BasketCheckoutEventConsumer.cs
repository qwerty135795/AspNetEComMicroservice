using AutoMapper;
using EventBusMessages.Events;
using MassTransit;
using MediatR;
using OrderingApplication.Features.Command.CheckoutOrder;

namespace OrderingAPI.Consumers;
public class BasketCheckoutEventConsumer : IConsumer<BasketCheckoutEvent>
{
    private readonly ILogger<BasketCheckoutEventConsumer> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public BasketCheckoutEventConsumer(ILogger<BasketCheckoutEventConsumer> logger,
    IMediator mediator, IMapper mapper)
    {

        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        _logger.LogInformation($"Consume message about {context.Message.UserName} Order");
        var result  = await _mediator.Send(_mapper.Map<CheckoutOrderCommand>(context.Message));
        _logger.LogInformation($"Id new order {result}");
    }
}
