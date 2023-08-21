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

        public Player? GetById(Guid id)
        {
            return _players.FirstOrDefault(p => p.Id == id);
        }

        public Player? GetByDeviceId(Guid deviceId)
        {
            return _players.FirstOrDefault(p => p.DeviceId == deviceId);
        }

        public void Create(Player player)
        {
            _players.Add(player);
        }
    }
}
