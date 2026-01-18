using Microsoft.EntityFrameworkCore;
using RatisbonaBackend.Business.Interfaces.Repositories;
using RatisbonaBackend.Infrastructure.Persistence;
using RatisbonaBackend.Infrastructure.Persistence.Repositories;

namespace RatisbonaBackend.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RatisbonaDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

