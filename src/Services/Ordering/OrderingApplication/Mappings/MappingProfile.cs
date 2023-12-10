using AutoMapper;
using OrderingApplication.Features.Command.CheckoutOrder;
using OrderingApplication.Features.Command.UpdateOrder;
using OrderingApplication.Features.Queries.GetOrdersList;
using OrderingDomain.Entities;

namespace OrderingApplication.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrdersVm>().ReverseMap();
        CreateMap<CheckoutOrderCommand, Order>().ReverseMap();
        CreateMap<UpdateOrderCommand, Order>().ReverseMap();
    }
}
