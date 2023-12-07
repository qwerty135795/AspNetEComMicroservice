using AutoMapper;
using DiscountGrpc.Entities;
using DiscountGrpc.Protos;

namespace DiscountGrpc.Infrastructure;
public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<Coupon,CouponModel>().ReverseMap();
    }
}
