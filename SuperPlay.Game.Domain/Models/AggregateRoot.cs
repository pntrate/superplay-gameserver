using SuperPlay.Game.Domain.Abstraction;

namespace SuperPlay.Game.Domain.Models
{
    public class AggregateRoot<T> : IAggregateRoot where T : struct
    {
        public T Id { get; protected set; }
    }
}