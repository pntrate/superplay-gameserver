using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers
{
    public abstract class MessageHandlerBase<TRequest, TResponse> : IMessageHandler<TRequest, TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, OperationContext context);
        public async Task<object?> Handle(object request, OperationContext context) => await Handle((TRequest) request, context);

        protected readonly IPlayerRepository _playerRepository;
        protected readonly IPlayerPool _playerPool;

        public MessageHandlerBase(IPlayerRepository playerRepository, IPlayerPool playerPool)
        {
            _playerRepository = playerRepository;
            _playerPool = playerPool;
        }
        
    }
}