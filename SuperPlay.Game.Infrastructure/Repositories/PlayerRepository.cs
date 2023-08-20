using SuperPlay.Game.Domain.Models.Players;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Infrastructure.Repositories
{
    public class PlayerRepository : Repository, IPlayerRepository
    {
        private readonly List<Player> _players;

        public PlayerRepository()
        {
            _players = new List<Player>();
        }

        public Player? Get(Guid id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public void Create(Player player)
        {
            _players.Add(player);
        }
    }
}
