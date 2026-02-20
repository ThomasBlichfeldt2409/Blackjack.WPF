using System.ComponentModel;

namespace Blackjack.Core
{
    public class Player : INotifyPropertyChanged
    {
        public string Name { get; set; } = string.Empty;
        private int _bank;
        public int Bank
        {
            get => _bank;
            set
            {
                if (_bank != value)
                {
                    _bank = value;
                    OnPropertyChanged(nameof(Bank));
                }
            }
        }
        public int BiggestWin { get; set; }
        public int BiggestLoose { get; set; }
        public int Total { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Player(string name)
        {
            Name = name;
            Bank = 1000;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}