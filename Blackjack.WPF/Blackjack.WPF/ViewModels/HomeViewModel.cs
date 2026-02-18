using Blackjack.Data;

namespace Blackjack.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;

        public HomeViewModel(PlayerRepository repository)
        {
            _repository = repository;
        }
    }
}