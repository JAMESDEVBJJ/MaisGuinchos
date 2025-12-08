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
    }
}
