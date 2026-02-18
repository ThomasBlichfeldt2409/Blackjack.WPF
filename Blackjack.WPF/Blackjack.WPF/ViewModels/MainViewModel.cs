using Blackjack.Data;
using Blackjack.WPF.Commands;
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

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateGameCommand { get; }

        public MainViewModel()
        {
            _repository = new PlayerRepository(App.DbPath);

            _homeVM = new HomeViewModel(_repository);
            _gameVM = new GameViewModel(_repository);

            NavigateHomeCommand = new RelayCommand(_ => NavigateHome());
            NavigateGameCommand = new RelayCommand(_ => NavigateGame());

            NavigateHome();
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
