using Lavadoras.Application.Common.JWT;
using Lavadoras.Application.IRepositories;
using Lavadoras.Domain.Crosscuting;
using Lavadoras.Persistence.Email;
using Lavadoras.Persistence.JWT;
using Lavadoras.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lavadoras.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtToken, JwtToken>();

        services.Configure<GmailSettings>(configuration.GetSection(GmailSettings.SectionName));
        services.AddScoped<IEmail, Gmail>();

        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}
