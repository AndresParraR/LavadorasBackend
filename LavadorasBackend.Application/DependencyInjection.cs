using Lavadoras.Application.Services.Auth;
using Lavadoras.Application.Services.Operator;
using Microsoft.Extensions.DependencyInjection;

namespace Lavadoras.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IOperatorService, OperatorService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
