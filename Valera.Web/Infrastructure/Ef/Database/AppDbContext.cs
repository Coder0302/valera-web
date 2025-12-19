using Microsoft.EntityFrameworkCore;
using ValeraWeb.Infrastructure.Ef.Entities;
using ValeraWeb.Infrastructure.Ef.Entities;

namespace ValeraWeb.Infrastructure.Ef.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ValeraEntity> Valeras => Set<ValeraEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<UserEntity>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
        });

        b.Entity<ValeraEntity>(e =>
        {
            e.HasOne(v => v.User)
             .WithMany(u => u.Valeras)
             .HasForeignKey(v => v.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
