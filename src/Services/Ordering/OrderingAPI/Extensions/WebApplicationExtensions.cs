using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderingInfrastructure.Persistence;

namespace OrderingAPI.Extensions;
public static class WebApplicationExtensions
{
    public static async Task MigrateDatabase<TContext>(this WebApplication app,
    Action<TContext, IServiceProvider> seed, int? retry = 0) where TContext : DbContext
    {
        int retryValue = retry.Value;
        var scope = app.Services.CreateScope();
        ILogger<TContext> logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
        var db = scope.ServiceProvider.GetRequiredService<TContext>();
        try
        {
            await db.Database.MigrateAsync();
            seed(db, scope.ServiceProvider);
        }
        catch (SqlException ex)
        {
            if (retryValue < 50)
            {
                logger.LogError(ex.Message);
                retry++;
                Thread.Sleep(2000);
                await MigrateDatabase<TContext>(app, seed, retry);
            }
        }
    }
}
