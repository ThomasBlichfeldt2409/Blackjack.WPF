using Blackjack.WPF.Commands;
using Blackjack.WPF.Views;
using System.Windows.Input;

namespace Blackjack.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel? _currentViewModel;

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
            NavigateHomeCommand = new RelayCommand(_ => NavigateHome());
            NavigateGameCommand = new RelayCommand(_ => NavigateGame());

            NavigateHome();
        }

        private void NavigateHome()
        {
            CurrentViewModel = new HomeViewModel();
        }

        private void NavigateGame()
        {
            CurrentViewModel = new GameViewModel();
        }
    }
}
