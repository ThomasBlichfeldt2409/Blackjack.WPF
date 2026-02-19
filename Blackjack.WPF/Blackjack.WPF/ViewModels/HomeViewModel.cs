using Blackjack.Core;
using Blackjack.Data;
using Blackjack.WPF.Commands;
using Blackjack.WPF.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;
        private bool _isLoaded;
        private Player? _selectedTablePlayer;

        public ObservableCollection<Player> AllPlayers { get; } = new();
        public ObservableCollection<Player> TablePlayers { get; }

        public Player? SelectedTablePlayer
        {
            get => _selectedTablePlayer;
            set
            {
                if (SetProperty(ref _selectedTablePlayer, value))
                {
                    (RemoveTablePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand RemoveTablePlayerCommand { get; }
        public ICommand OpenCreatePlayerCommand { get; }

        public HomeViewModel(PlayerRepository repository, ObservableCollection<Player> tablePlayers)
        {
            _repository = repository;
            TablePlayers = tablePlayers;

            // Commands  
            RemoveTablePlayerCommand = new RelayCommand(
                _ => RemoveTablePlayer(), 
                _ => SelectedTablePlayer != null
            );

            OpenCreatePlayerCommand = new RelayCommand(_ => OpenCreatePlayer());
        }

        private void RemoveTablePlayer()
        {
            if (SelectedTablePlayer == null)
                return;

            TablePlayers.Remove(SelectedTablePlayer);
        }

        private void OpenCreatePlayer()
        {
            CreatePlayerViewModel vm = new CreatePlayerViewModel();

            CreatePlayerWindow window = new CreatePlayerWindow
            {
                Owner = App.Current.MainWindow,
                DataContext = vm
            };

            if (window.ShowDialog() == true)
            {
                AllPlayers.Add(vm.CreatedPlayer!);
            }
        }

        public async Task LoadPlayersAsync()
        {
            if (_isLoaded)
                return;

            List<Player> players = await _repository.GetAllAsync();

            AllPlayers.Clear();
            foreach (Player player in players)
            {
                AllPlayers.Add(player);
            }

            _isLoaded = true;
        }
    }
}