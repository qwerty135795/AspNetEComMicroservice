using Dapper;
using DiscountAPI.Entities;
using Npgsql;

namespace DiscountAPI.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IConfiguration _configuration;

    public DiscountRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        var result = await connection.ExecuteAsync("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
        new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount});

        return result != 0;
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>("SELECT * FROM Coupon WHERE ProductName = @ProductName",
        new {ProductName = productName});
        if (coupon is null) return new Coupon { ProductName = "No Discount", Description = "No Discount Desc", Amount = 0};
        return coupon;
    }

    public async Task<bool> RemoveDiscount(string productName)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        
        var result = await connection.ExecuteAsync("DELETE FROM Coupon Where ProductName = @ProductName",
        new {ProductName = productName});

        return result != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSetting:ConnectionString"));
        var result = await connection.ExecuteAsync("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount Where Id = @Id",
        new {ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id});

        return result != 0;
    }
}