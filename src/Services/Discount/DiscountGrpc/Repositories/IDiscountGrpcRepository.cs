using DiscountGrpc.Entities;

namespace DiscountGrpc.Repositories;
public interface IDiscountGrpcRepository
{
    Task<Coupon> GetDiscount(string productName);
    Task<bool> CreateDiscount(Coupon coupon);
    Task<bool> UpdateDiscount(Coupon coupon);
    Task<bool> RemoveDiscount(string productName);
}
