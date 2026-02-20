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
        private Player? _selectedPlayer;
        private Player? _selectedTablePlayer;

        public ObservableCollection<Player> AllPlayers { get; } = new();
        public ObservableCollection<Player> TablePlayers { get; }

        public Player? SelectedPlayer
        {
            get => _selectedPlayer;
            set
            {
                if (SetProperty(ref _selectedPlayer, value))
                {
                    (DeletePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

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
        public ICommand DeletePlayerCommand { get; }
        public ICommand AddToTableCommand { get; }
        public ICommand GetMoneyCommand { get; }

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

            DeletePlayerCommand = new RelayCommand(
                _ => DeletePlayer(),
                _ => CanDeletePlayer()
            );

            AddToTableCommand = new RelayCommand(
                _ => AddToTable(),
                _ => CanAddToTable()
            );

            GetMoneyCommand = new RelayCommand(
                _ => GetMoney(),
                _ => CanGetMoney()
            );
        }

        private void RemoveTablePlayer()
        {
            if (SelectedTablePlayer == null)
                return;

            TablePlayers.Remove(SelectedTablePlayer);

            // Re-evaluate so button disables
            (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeletePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void OpenCreatePlayer()
        {
            List<string> existingNames = AllPlayers
                .Select(p => p.Name)
                .ToList();

            CreatePlayerViewModel vm = new CreatePlayerViewModel(existingNames, _repository);

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

        private async void DeletePlayer()
        {
            if (SelectedPlayer == null)
                return;

            await _repository.DeleteAsync(SelectedPlayer);

            AllPlayers.Remove(SelectedPlayer);

            SelectedPlayer = null;
        }

        private bool CanDeletePlayer()
        {
            if (SelectedPlayer == null)
                return false;

            if (TablePlayers.Contains(SelectedPlayer))
                return false;

            return true;
        }

        private void AddToTable()
        {
            if (SelectedPlayer == null)
                return;

            TablePlayers.Add(SelectedPlayer);

            // Re-evaluate so button disables
            (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeletePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanAddToTable()
        {
            if (SelectedPlayer == null)
                return false;

            if (SelectedPlayer.Bank <= 0)
                return false;

            if (TablePlayers.Count >= 4)
                return false;

            return !TablePlayers.Any(p => p.Name == SelectedPlayer.Name);
        }

        private async void GetMoney()
        {
            if (SelectedPlayer == null)
                return;

            SelectedPlayer!.Bank += 1000;

            await _repository.UpdateAsync(SelectedPlayer);

            // Re-evaluate so button disables
            (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanGetMoney()
        {
            if (SelectedPlayer == null)
                return false;

            if (SelectedPlayer!.Bank > 0)
                return false;

            return true;
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