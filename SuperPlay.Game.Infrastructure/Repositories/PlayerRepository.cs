using SuperPlay.Game.Domain.Models.Players;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Infrastructure.Repositories
{
    public class PlayerRepository : Repository, IPlayerRepository
    {
        private readonly List<Player> _players;

        public PlayerRepository()
        {
            //seeding data for testing client
            _players = new List<Player>
            {
                new Player(Guid.Parse("9e7cebc7-6961-4725-b5da-c2ae16319be8"), Guid.Parse("89af07a5-75ca-4aa0-be9b-6b0a8686c2e5")),
                new Player(Guid.Parse("35d6e601-1a24-47ab-ac6d-9a92928e4ef3"), Guid.Parse("805987ab-5785-4609-9eca-43fb1305cd4f")),
                new Player(Guid.Parse("62b8fe24-e0bf-4083-8566-b6886ee600a7"), Guid.Parse("8b403820-f525-4bd1-bb92-544e62a63856")),
            };
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
