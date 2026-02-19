using Blackjack.Core;
using Blackjack.WPF.Commands;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class CreatePlayerViewModel : BaseViewModel
    {
        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Player? CreatedPlayer { get; private set; }

        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool?>? CloseRequested;

        public CreatePlayerViewModel()
        {
            CreateCommand = new RelayCommand(
                _ => Create(),
                _ => !string.IsNullOrWhiteSpace(Name)
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Create()
        {
            CreatedPlayer = new Player(Name!);
            CloseRequested?.Invoke(true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}
