using Blackjack.Core;
using Blackjack.Data;
using Blackjack.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;

        private BaseViewModel? _currentViewModel;
        private readonly HomeViewModel _homeVM;
        private readonly GameViewModel _gameVM;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel!;
            set
            { 
                if (SetProperty(ref _currentViewModel, value))
                {
                    OnPropertyChanged(nameof(IsHomeView));
                    OnPropertyChanged(nameof(IsGameView));
                }
            }
        }

        public bool IsHomeView =>
            CurrentViewModel is HomeViewModel;

        public bool IsGameView =>
            CurrentViewModel is GameViewModel;

        public ObservableCollection<Player> TablePlayers { get; } = new();

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateGameCommand { get; }

        public MainViewModel()
        {
            _repository = new PlayerRepository(App.DbPath);

            _homeVM = new HomeViewModel(_repository, TablePlayers);
            _gameVM = new GameViewModel(_repository, TablePlayers);

            NavigateHomeCommand = new RelayCommand(_ => NavigateHome());
            NavigateGameCommand = new RelayCommand(_ => NavigateGame());

            NavigateHome();

            TablePlayers.Add(new Player { Name = "Thomas", Bank = 1000 });
            TablePlayers.Add(new Player { Name = "Holger", Bank = 3200 });
            TablePlayers.Add(new Player { Name = "Carl", Bank = 1600 });
            TablePlayers.Add(new Player { Name = "Lakrids", Bank = 200 });
        }

        private void NavigateHome()
        {
            CurrentViewModel = _homeVM;
        }

        private void NavigateGame()
        {
            CurrentViewModel = _gameVM;
        }
    }
}
