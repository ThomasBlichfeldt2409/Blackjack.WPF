namespace Blackjack.Core
{
    public class Player
    {
        public string Name { get; set; } = string.Empty;
        public int Bank { get; set; }
        public int BiggestWin { get; set; }
        public int BiggestLoose { get; set; }
        public int Total { get; set; }

        public Player(string name)
        {
            Name = name;
            Bank = 1000;
        }
    }
}