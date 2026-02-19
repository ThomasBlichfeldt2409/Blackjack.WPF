using Blackjack.Core;
using Blackjack.Data;
using System.Collections.ObjectModel;

namespace Blackjack.WPF.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly PlayerRepository _repository;
        private bool _isLoaded;

        public ObservableCollection<Player> AllPlayers { get; } = new();
        public ObservableCollection<Player> TablePlayers { get; }

        public HomeViewModel(PlayerRepository repository, ObservableCollection<Player> tablePlayers)
        {
            _repository = repository;
            TablePlayers = tablePlayers;
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