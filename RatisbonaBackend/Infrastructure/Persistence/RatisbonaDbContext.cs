using Microsoft.EntityFrameworkCore;
using RatisbonaBackend.Business.Entities;
using RatisbonaBackend.Infrastructure.Persistence.Configurations;

namespace RatisbonaBackend.Infrastructure.Persistence;

public class RatisbonaDbContext : DbContext
{
    public RatisbonaDbContext(DbContextOptions<RatisbonaDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}