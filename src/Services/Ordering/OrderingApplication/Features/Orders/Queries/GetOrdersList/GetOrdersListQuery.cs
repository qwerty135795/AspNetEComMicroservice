using MediatR;

namespace OrderingApplication.Features.Queries.GetOrdersList;
public class GetOrdersListQuery : IRequest<List<OrdersVm>>
{
    public GetOrdersListQuery(string userName)
    {
        UserName = userName;
    }

    public string UserName { get; set; }
}
