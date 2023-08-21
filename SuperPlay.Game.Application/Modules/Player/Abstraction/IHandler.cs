using SuperPlay.Game.Application.Modules.Player.Models.Common;

namespace SuperPlay.Game.Application.Modules.Player.Abstraction
{
    public interface IHandler
    {
        Task<object?> Handle(object request, PlayerContext context);
    }

    public interface IHandler<TRequest, TResponse> : IHandler
    {
        Task<TResponse> Handle(TRequest request, PlayerContext context);
    }
}