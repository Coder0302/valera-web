using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using ValeraWeb.Domain.Entities;
using ValeraWeb.Infrastructure.Ef.Entities;
using ValeraWeb.Migrations;

namespace ValeraWeb.Infrastructure.Ef.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ValeraEntity> Valeras => Set<ValeraEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Email).IsRequired();
            b.Property(x => x.EmailNormalized).IsRequired();
            b.Property(x => x.Username).IsRequired();
            b.Property(x => x.PasswordHash).IsRequired();
            b.Property(x => x.Role).IsRequired();
        });

        modelBuilder.Entity<ValeraEntity>(b =>
        {
            b.HasKey(x => x.Id);

            b.HasOne(x => x.User)
             .WithMany()
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasIndex(x => x.UserId);
        });
    }
}
