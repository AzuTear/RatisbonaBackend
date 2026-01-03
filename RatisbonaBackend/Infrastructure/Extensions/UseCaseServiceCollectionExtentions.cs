namespace RatisbonaBackend.Infrastructure.Extensions;

public static class UseCaseServiceCollectionExtentions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<LoginUserHandler>();

        return services;
    }
}