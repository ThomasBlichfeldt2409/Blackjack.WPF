using Blackjack.Core;
using Blackjack.Data;
using Blackjack.WPF.Commands;
using Blackjack.WPF.Enums;
using Blackjack.WPF.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;
        private bool _isLoaded;
        private bool _isTyping;
        private Player? _selectedPlayer;
        private Player? _selectedTablePlayer;
        private HomeState _currentState;
        private HomeState _lastTypedState;
        private string _casinoMessage = string.Empty;

        public string CasinoMessage
        {
            get => _casinoMessage;
            set => SetProperty(ref _casinoMessage, value);
        }

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

        private async void RemoveTablePlayer()
        {
            if (SelectedTablePlayer == null)
                return;

            TablePlayers.Remove(SelectedTablePlayer);

            // Re-evaluate so button disables
            (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeletePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();

            await RefreshStateAsync();
        }

        private async void OpenCreatePlayer()
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
                await RefreshStateAsync();
            }
        }

        private async void DeletePlayer()
        {
            if (SelectedPlayer == null)
                return;

            await _repository.DeleteAsync(SelectedPlayer);

            AllPlayers.Remove(SelectedPlayer);

            SelectedPlayer = null;

            await RefreshStateAsync();
        }

        private bool CanDeletePlayer()
        {
            if (SelectedPlayer == null)
                return false;

            if (TablePlayers.Contains(SelectedPlayer))
                return false;

            return true;
        }

        private async void AddToTable()
        {
            if (SelectedPlayer == null)
                return;

            TablePlayers.Add(SelectedPlayer);

            // Re-evaluate so button disables
            (AddToTableCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeletePlayerCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (GetMoneyCommand as RelayCommand)?.RaiseCanExecuteChanged();

            await RefreshStateAsync();
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

            await RefreshStateAsync();
        }

        private bool CanGetMoney()
        {
            if (SelectedPlayer == null)
                return false;

            if (SelectedPlayer!.Bank > 0)
                return false;

            return true;
        }

        private HomeState EvaluateState()
        {
            if (!AllPlayers.Any())
                return HomeState.NoActivePlayers;

            if (AllPlayers.All(p => p.Bank <= 0))
                return HomeState.NoMoney;

            if (!TablePlayers.Any())
                return HomeState.NoTablePlayers;

            return HomeState.GameReady;
        }

        private async Task RefreshStateAsync()
        {
            _currentState = EvaluateState();

            if (_currentState == _lastTypedState || _isTyping == true)
                return;

            _isTyping = true;
            _lastTypedState = _currentState;

            await ShowStateMessageAsync(_currentState);

            _isTyping = false;

            await RefreshStateAsync();
        }

        private async Task ShowStateMessageAsync(HomeState state)
        {
            switch (state)
            {
                case HomeState.Startup:
                    await TypeMessageAsync("Welcome to the Blackjack Lounge. Take a seat, gather your players, and let’s see if luck is on your side tonight.");
                    break;

                case HomeState.NoActivePlayers:
                    await TypeMessageAsync("It looks a little empty in here… Create a player to step up to the table.");
                    break;

                case HomeState.NoMoney:
                    await TypeMessageAsync("Your players are out of chips! Give them some funds before they can join the action.");
                    break;

                case HomeState.NoTablePlayers:
                    await TypeMessageAsync("The table is waiting. Add at least one player to begin the game.");
                    break;

                case HomeState.GameReady:
                    await TypeMessageAsync("At least one player has been added to the table. You may start the game.");
                    break;
            }
        }

        private async Task TypeMessageAsync(string message, int delay = 50)
        {
            CasinoMessage = string.Empty;

            foreach (char c in message)
            {
                CasinoMessage += c;
                await Task.Delay(delay);
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

        public async Task InitializeStateAsync()
        {
            _lastTypedState = HomeState.Startup;
            _isTyping = true;

            await ShowStateMessageAsync(HomeState.Startup);
            await Task.Delay(3000);
            _isTyping = false;

            await RefreshStateAsync();

        }
    }
}