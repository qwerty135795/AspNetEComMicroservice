using DiscountGrpc.Extensions;
using DiscountGrpc.Repositories;
using DiscountGrpc.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IDiscountGrpcRepository, DiscountGrpcRepository>();
builder.WebHost.ConfigureKestrel(opt =>
{
    opt.ListenAnyIP(builder.Configuration.GetValue<int>("port"), o => o.Protocols = HttpProtocols.Http2);
});
var app = builder.Build();
app.MigrateDatabase<Program>();
// Configure the HTTP request pipeline.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
