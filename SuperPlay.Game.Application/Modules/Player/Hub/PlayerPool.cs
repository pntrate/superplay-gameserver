using SuperPlay.Game.Application.Modules.Player.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Hub
{
    public class PlayerPool : IPlayerPool
    {
        private readonly Dictionary<Guid, PlayerChannel> _players;

        public PlayerPool()
        {
            _players = new();
        }

        public void PlayerLoggedIn(Guid playerId, PlayerChannel channel)
        {
            _players[playerId] = channel;
            channel.OnClose += () => PlayerLoggedOut(playerId);
        }

        public void PlayerLoggedOut(Guid playerId)
        {
            _players.Remove(playerId);
        }

        public async Task SendMessageTo<T>(Guid playerId, T message)
        {
            var channel = _players[playerId];
            await channel.SendMessage(message);
        }

    }
}