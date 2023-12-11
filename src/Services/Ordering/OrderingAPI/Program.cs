using EventBusMessages.Common;
using EventBusMessages.Events;
using MassTransit;
using OrderingAPI.Consumers;
using OrderingAPI.Extensions;
using OrderingApplication;
using OrderingInfrastructure;
using OrderingInfrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(opt => 
{
    opt.AddConsumersFromNamespaceContaining<BasketCheckoutEventConsumer>();
    opt.UsingRabbitMq((ctx,cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", config =>
        {
            config.Username("guest");
            config.Password("guest");
        });
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutEventConsumer>(ctx);
        });
        cfg.ConfigureEndpoints(ctx);
    });
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<BasketCheckoutEventConsumer>();
var app = builder.Build();
await app.MigrateDatabase<OrderContext>((a,b) => OrderContextSeed.SeedData(a)); 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
