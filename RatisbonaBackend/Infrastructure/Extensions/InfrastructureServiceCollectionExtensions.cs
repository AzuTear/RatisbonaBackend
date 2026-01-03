using RatisbonaBackend.Business.Interfaces.Repositories;
using RatisbonaBackend.Infrastructure.Persistence;
using RatisbonaBackend.Infrastructure.Persistence.Repositories;

namespace RatisbonaBackend.Infrastructure.Extensions;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext<RatisbonaDbContext>(UseNpgsql);
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
