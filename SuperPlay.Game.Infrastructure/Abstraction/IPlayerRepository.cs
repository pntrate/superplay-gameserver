using SuperPlay.Game.Domain.Models.Players;

namespace SuperPlay.Game.Infrastructure.Abstraction
{
    public interface IPlayerRepository : IRepository
    {
        Player? Get(Guid id);
        void Create(Player player);
    }
}
