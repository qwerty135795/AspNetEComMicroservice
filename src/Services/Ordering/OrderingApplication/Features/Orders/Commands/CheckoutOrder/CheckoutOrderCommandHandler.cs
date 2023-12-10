using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using OrderingApplication.Contracts.Infrastructure;
using OrderingApplication.Contracts.Persistence;
using OrderingApplication.Models;
using OrderingDomain.Entities;

namespace OrderingApplication.Features.Command.CheckoutOrder;
public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;

    public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        var result = await _orderRepository.AddAsync(_mapper.Map<Order>(request));
        _logger.LogInformation($"Order {result.Id} is successfully created.");
        await SendMail(result);
        return result.Id;
    }

    private async Task SendMail(Order order)
    {
        var email = new Email { To = order.EmailAddress, Subject = "Order created", Body = "Order was created"};
        try
        {
            await _emailService.SendEmail(email);
        }
        catch(Exception ex)
        {
            _logger.LogError($"Order {order.Id} failed duo to an error with the mail service: {ex.Message}");
        }
    }
}
