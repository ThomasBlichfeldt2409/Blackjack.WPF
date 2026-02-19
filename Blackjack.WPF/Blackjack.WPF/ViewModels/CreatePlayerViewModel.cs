using Blackjack.Core;
using Blackjack.Data;
using Blackjack.WPF.Commands;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class CreatePlayerViewModel : BaseViewModel
    {
        private readonly HashSet<string> _existingNames;
        private readonly PlayerRepository _repository;

        private string? _name;
        public string? Name
        {
            get => _name;
            set
            {
                if (SetProperty(ref _name, value))
                {
                    (CreateCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public Player? CreatedPlayer { get; private set; }

        public ICommand CreateCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool?>? CloseRequested;

        public CreatePlayerViewModel(List<string> existingNames, PlayerRepository repository)
        {
            _existingNames = new HashSet<string>(existingNames.Select(n => n.ToLower()));
            _repository = repository;

            CreateCommand = new RelayCommand(
                _ => Create(),
                _ => CanCreate()
            );

            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private async void Create()
        {
            string name = Name!.Trim();
            Player player = new Player(name);

            await _repository.AddAsync(player);

            CreatedPlayer = player;
            CloseRequested?.Invoke(true);
        }

        private bool CanCreate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            string name = Name.Trim();

            if (name.Length > 20)
                return false;

            if (_existingNames.Contains(name.ToLower()))
                return false;

            return true;
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(false);
        }
    }
}
