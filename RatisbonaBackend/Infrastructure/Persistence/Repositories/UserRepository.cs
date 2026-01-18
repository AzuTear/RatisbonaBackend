using Microsoft.EntityFrameworkCore;
using RatisbonaBackend.Business.Entities;
using RatisbonaBackend.Business.Interfaces.Repositories;

namespace RatisbonaBackend.Infrastructure.Persistence.Repositories;

public class UserRepository(RatisbonaDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Users.FindAsync([id], cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<List<User>> ListAsync(CancellationToken cancellationToken)
    {
        return await context.Users
            .AsNoTracking()
            .OrderBy(user => user.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([id], cancellationToken);
        if (user is null)
        {
            return;
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
