using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderingApplication.Behaviors;
using OrderingApplication.Exceptions;

namespace OrderingApplication;
public static class ApplicationServiceRegistration
{
    public static void AddApplicationService(this IServiceCollection services)
    {
        // if not working add Assembly.GetExecutingAssembly();
        services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehavior<,>));
    }
}
