using Blackjack.Core;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data
{
    public class BlackjackDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        private readonly string? _databasePath;

        public BlackjackDbContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(p => p.Name);
                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Bank).IsRequired();
                entity.Property(p => p.BiggestWin).IsRequired();
                entity.Property(p => p.BiggestLoose).IsRequired();
                entity.Property(p => p.Total).IsRequired();
            });
        }
    }
}
