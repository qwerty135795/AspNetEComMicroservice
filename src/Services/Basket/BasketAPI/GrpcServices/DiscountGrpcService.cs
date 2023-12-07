using DiscountGrpc.Protos;

namespace GrpcServices;

public class DiscountGrpcService 
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _protoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient protoService)
    {
        _protoService = protoService;
    }
    public async Task<CouponModel> GetDiscount(string ProductName)
    {
        return await _protoService.GetDiscountAsync(new GetDiscountRequest { ProductName = ProductName});
    }
}