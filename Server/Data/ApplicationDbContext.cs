using CSMessagingApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CSMessagingApp.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Agent> Agents { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.AssignedAgent)
                .WithMany()
                .HasForeignKey(m => m.AssignedAgentId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
