using AutoMapper;
using EventBusMessages.Events;
using OrderingApplication.Features.Command.CheckoutOrder;

namespace OrderingAPI.Mapper;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>().ReverseMap();
    }
}
