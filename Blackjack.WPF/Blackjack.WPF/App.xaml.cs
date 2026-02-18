using Blackjack.Data;
using System.IO;
using System.Windows;

namespace Blackjack.WPF
{
    public partial class App : Application
    {
        public static string DbPath { get; private set; } = string.Empty;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set the database path
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blackjack");
            Directory.CreateDirectory(folder);

            DbPath = Path.Combine(folder, "blackjack.db");

            // Creating / Connecting to the Database
            using BlackjackDbContext context = new BlackjackDbContext(DbPath);
            context.Database.EnsureCreated();
        }
    }
}
