using Dapper;
using DiscountGrpc.Entities;
using Npgsql;

namespace DiscountGrpc.Repositories;
public class DiscountGrpcRepository : IDiscountGrpcRepository
{
    private readonly IConfiguration _conf;

    public DiscountGrpcRepository(IConfiguration conf)
    {
        _conf = conf;
    }
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        var connection = new NpgsqlConnection(_conf.GetValue<string>("DatabaseSetting:ConnectionString"));

        return await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
        new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount }) != 0;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        var connection = new NpgsqlConnection(_conf.GetValue<string>("DatabaseSetting:ConnectionString"));

        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName",
        new { ProductName = productName});

        return coupon ?? new Coupon { Id = 1, ProductName = "No Discount", Description = "No Discount", Amount = 0};
    }

    public async Task<bool> RemoveDiscount(string productName)
    {
        var connection = new NpgsqlConnection(_conf.GetValue<string>("DatabaseSetting:ConnectionString"));

        return await connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
        new { ProductName = productName }) != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        var connection = new NpgsqlConnection(_conf.GetValue<string>("DatabaseSetting:ConnectionString"));

        return await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
        new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id}) != 0;
    }
}
