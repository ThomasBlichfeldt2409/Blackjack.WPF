using System.Windows.Input;
using Blackjack.WPF.Commands;

namespace Blackjack.WPF.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateGameCommand { get; }

        public MainViewModel()
        {
            NavigateHomeCommand = new RelayCommand(_ => NavigateHome());
            NavigateGameCommand = new RelayCommand(_ => NavigateGame());
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
