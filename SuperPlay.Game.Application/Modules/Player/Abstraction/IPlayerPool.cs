using SuperPlay.Game.Application.Modules.Player.Hub;

namespace SuperPlay.Game.Application.Modules.Player.Abstraction
{
    public interface IPlayerPool
    {
        void PlayerLoggedIn(Guid playerId, PlayerChannel channel);
        void PlayerLoggedOut(Guid playerId);
        Task SendMessageTo<T>(Guid playerId, T message);
    }
}