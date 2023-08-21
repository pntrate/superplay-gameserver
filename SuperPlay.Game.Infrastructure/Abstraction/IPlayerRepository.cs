using SuperPlay.Game.Domain.Models.Players;

namespace SuperPlay.Game.Infrastructure.Abstraction
{
    public interface IPlayerRepository : IRepository
    {
        Player? GetById(Guid id);
        Player? GetByDeviceId(Guid deviceId);
        void Create(Player player);
    }
}
