using Blackjack.Data;

namespace Blackjack.WPF.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;

        public GameViewModel(PlayerRepository repository)
        {
            _repository = repository;
        }
    }
}
