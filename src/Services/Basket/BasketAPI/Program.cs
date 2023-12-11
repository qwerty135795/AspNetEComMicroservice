using BasketAPI.Repositories;
using DiscountGrpc.Protos;
using GrpcServices;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddStackExchangeRedisCache(opt => 
{
    opt.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
});
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(opt => 
{
    opt.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(opt => 
{

    opt.UsingRabbitMq((ctx, config) =>
    {
        config.Host(builder.Configuration["RabbitMq:Host"], "/", cfg => 
        {
            cfg.Username("guest");
            cfg.Password("guest");
        });
        config.ConfigureEndpoints(ctx);
    });
});
builder.Services.AddScoped<DiscountGrpcService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
