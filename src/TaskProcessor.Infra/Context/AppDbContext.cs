using Microsoft.EntityFrameworkCore;
using TaskProcessor.Domain.Model;

namespace TaskProcessor.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasKey(c => c.Id)
                .HasAnnotation("Sqlite:Autoincrement", true);

            modelBuilder.Entity<Address>()
                .HasKey(a => a.Id)
                .HasAnnotation("Sqlite:Autoincrement", true);
            base.OnModelCreating(modelBuilder);
        }
    }
}
