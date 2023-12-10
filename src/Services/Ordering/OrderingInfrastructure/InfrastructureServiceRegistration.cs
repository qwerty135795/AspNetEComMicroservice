using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderingApplication.Contracts.Infrastructure;
using OrderingApplication.Contracts.Persistence;
using OrderingApplication.Models;
using OrderingDomain.Entities;
using OrderingInfrastructure.Mail;
using OrderingInfrastructure.Persistence;
using OrderingInfrastructure.Repositories;

namespace OrderingInfrastructure;
public static class InfrastructureServiceRegistration
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<OrderContext>(opt => 
        {
            opt.UseSqlServer(config.GetConnectionString("SqlServer"));
        });
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.Configure<EmailSettings>(opt => config.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();
    }
}
