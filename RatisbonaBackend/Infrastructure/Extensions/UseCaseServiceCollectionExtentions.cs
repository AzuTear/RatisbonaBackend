using RatisbonaBackend.Business.Services;

namespace RatisbonaBackend.Infrastructure.Extensions;

public static class UseCaseServiceCollectionExtentions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<UsersService>();

        return services;
    }
}
