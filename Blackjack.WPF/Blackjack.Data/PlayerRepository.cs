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
    }
}
