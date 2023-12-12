
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console().CreateLogger();
try 
{
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json",true,true);
builder.Services.AddOcelot().AddCacheManager(x => x.WithDictionaryHandle());
builder.Host.UseSerilog();
var app = builder.Build();
app.UseSerilogRequestLogging();
await app.UseOcelot();

app.Run();

}
catch(Exception ex)
{
    Log.Fatal(ex, "app crashed");
}
finally
{
    Log.CloseAndFlush();
}