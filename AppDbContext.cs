using Microsoft.EntityFrameworkCore;
using MaisGuinchos.Models;

namespace MaisGuinchos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Guincho> Guinchos { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<TowRequest> TowRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Guincho)
                .WithOne(g => g.User)
                .HasForeignKey<Guincho>(g => g.UserId);

            modelBuilder.Entity<Location>()
                .HasOne(l => l.User)
                .WithMany(u => u.Locations)
                .HasForeignKey(l => l.UserId);
            modelBuilder.Entity<TowRequest>()
                .HasOne(tr => tr.Client)
                .WithMany()
                .HasForeignKey(tr => tr.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TowRequest>()
                .HasOne(tr => tr.Driver)
                .WithMany()
                .HasForeignKey(tr => tr.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
