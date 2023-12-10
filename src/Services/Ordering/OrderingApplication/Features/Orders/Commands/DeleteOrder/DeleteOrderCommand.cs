using MediatR;

namespace OrderingApplication.Features.Command.DeleteOrder;
public class DeleteOrderCommand : IRequest
{
    public int Id { get; set; }
}
