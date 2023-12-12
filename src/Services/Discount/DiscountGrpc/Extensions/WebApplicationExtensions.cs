using Npgsql;

namespace DiscountGrpc.Extensions;
public static class WebApplicationExtensions
{
    public static void MigrateDatabase<TContext>(this WebApplication app, int? retry = 0) where TContext : class
    {
        using var scope = app.Services.CreateScope();

        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        try
        {
            logger.LogInformation("MigrateDatabase started");
            var connection = new NpgsqlConnection(config.GetValue<string>("DatabaseSetting:ConnectionString"));
            connection.Open();
            using var command = new NpgsqlCommand { Connection = connection};
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();
            command.CommandText = @"CREATE TABLE Coupon(
                Id SERIAL PRIMARY KEY,
                ProductName VARCHAR(24) NOT NULL,
                Description TEXT NOT NULL,
                Amount INT
            )";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150)";
            command.ExecuteNonQuery();
            command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100)";
            command.ExecuteNonQuery();
            connection.Close();
            logger.LogInformation("Migrate Database Completed");
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex.Message);
            while (retry < 50)
            {
                retry++;
                Thread.Sleep(2000);
                MigrateDatabase<TContext>(app, retry);
            }
        }
    }
}
