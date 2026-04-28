using Microsoft.EntityFrameworkCore;
using DevelopTest.Models;

namespace DevelopTest.Data;

public class MySqlDbContext : DbContext
{
    public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<SportArea> SportAreas { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasIndex(r => new { r.UserId, r.SportAreaId })
                .IsUnique();

            entity.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.SportArea)
                .WithMany()
                .HasForeignKey(r => r.SportAreaId) 
                .OnDelete(DeleteBehavior.Restrict);
        });
        
    }
}