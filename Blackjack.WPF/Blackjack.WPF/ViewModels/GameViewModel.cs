using Blackjack.Core;
using Blackjack.Data;
using System.Collections.ObjectModel;

namespace Blackjack.WPF.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;

        public ObservableCollection<Player> TablePlayers { get; }

        public GameViewModel(PlayerRepository repository, ObservableCollection<Player> tablePlayer)
        {
            _repository = repository;
            TablePlayers = tablePlayer;
        }
    }
}
