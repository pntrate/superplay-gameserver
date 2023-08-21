using SuperPlay.Game.Application.Modules.Player.Models.Common;

namespace SuperPlay.Game.Application.Modules.Player.Abstraction
{
    public interface IMessageHandler
    {
        Task<object?> Handle(object request, OperationContext context);
    }

    public interface IMessageHandler<TRequest, TResponse> : IMessageHandler
    {
        Task<TResponse> Handle(TRequest request, OperationContext context);
    }
}