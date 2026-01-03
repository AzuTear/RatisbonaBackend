using RatisbonaBackend.Business.Entities;
using RatisbonaBackend.Business.Interfaces.Repositories;

namespace RatisbonaBackend.Infrastructure.Persistence.Repositories;

public class UserRepository(RatisbonaDbContext context) : IUserRepository
{
    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.FindAsync(id);
    }
}