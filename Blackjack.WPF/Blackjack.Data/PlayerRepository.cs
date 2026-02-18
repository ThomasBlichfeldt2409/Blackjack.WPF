using Blackjack.Core;
using Microsoft.EntityFrameworkCore;

namespace Blackjack.Data
{
    public class PlayerRepository
    {
        private readonly string? _databasePath;

        public PlayerRepository(string databasePath)
        {
            _databasePath = databasePath; 
        }

        private BlackjackDbContext CreateContext()
        {
            return new BlackjackDbContext(_databasePath!);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            using BlackjackDbContext context = CreateContext();
            return await context.Players.ToListAsync();
        }

        public async Task AddAsync(Player player)
        {
            using BlackjackDbContext context = CreateContext();
            context.Players.Add(player);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Player player)
        {
            using BlackjackDbContext context = CreateContext();
            context.Players.Update(player);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Player player)
        {
            using BlackjackDbContext context = CreateContext();
            context.Players.Remove(player);
            await context.SaveChangesAsync();
        }
    }
}
