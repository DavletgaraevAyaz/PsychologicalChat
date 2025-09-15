using APIPsychologicalChat.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPsychologicalChat.DataBase
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<HelpSpecialist> HelpSpecialists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка индексов для улучшения производительности
            modelBuilder.Entity<ChatMessage>()
                .HasIndex(m => m.UserId);

            modelBuilder.Entity<ChatMessage>()
                .HasIndex(m => m.Timestamp);

            modelBuilder.Entity<MoodEntry>()
                .HasIndex(m => m.UserId);

            modelBuilder.Entity<MoodEntry>()
                .HasIndex(m => m.CreatedAt);
        }
    }
}
