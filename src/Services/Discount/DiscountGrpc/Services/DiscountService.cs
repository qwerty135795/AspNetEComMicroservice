using AutoMapper;
using DiscountGrpc.Entities;
using DiscountGrpc.Protos;
using DiscountGrpc.Repositories;
using Grpc.Core;

namespace DiscountGrpc.Services;
public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountGrpcRepository _repo;
    private readonly ILogger<DiscountService> _logger;
    private readonly IMapper _mapper;

    public DiscountService(IDiscountGrpcRepository repo, ILogger<DiscountService> logger,
    IMapper mapper)
    {
        _repo = repo;
        _logger = logger;
        _mapper = mapper;
    }
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repo.GetDiscount(request.ProductName) ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with product Name {request.ProductName} not found"));
        return _mapper.Map<CouponModel>(coupon);
    }
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        await _repo.CreateDiscount(_mapper.Map<Coupon>(request.Coupon));

        return request.Coupon;
    }
    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        if(await _repo.UpdateDiscount(_mapper.Map<Coupon>(request.Coupon))) return request.Coupon;
        throw new RpcException(new Status(StatusCode.Internal, "Internal error"));
    }
    public override async Task<RemoveResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var result = new RemoveResponse { Result = await _repo.RemoveDiscount(request.ProductName)};
        return result;
    }
}
