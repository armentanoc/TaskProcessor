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
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<SubTaskEntity> SubTasks{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>().HasKey(c => c.Id);

            modelBuilder.Entity<SubTaskEntity>().HasKey(a => a.Id);

            base.OnModelCreating(modelBuilder);
        }
    }
}
