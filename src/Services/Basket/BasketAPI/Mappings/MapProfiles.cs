using AutoMapper;
using BasketAPI.Entities;
using EventBusMessages.Events;

namespace BasketAPI.Mappings;
public class MapProfiles : Profile
{
    public MapProfiles()
    {
        CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
    }
}
