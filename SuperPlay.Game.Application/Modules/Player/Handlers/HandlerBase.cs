using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers
{
    public abstract class HandlerBase<TRequest, TResponse> : IHandler<TRequest, TResponse>
    {
        public abstract Task<TResponse> Handle(TRequest request, PlayerContext context);
        public async Task<object?> Handle(object request, PlayerContext context) => await Handle(request, context);

        protected readonly IPlayerRepository _playerRepository;
        protected readonly IPlayerPool _playerPool;

        public HandlerBase(IPlayerRepository playerRepository, IPlayerPool playerPool)
        {
            _playerRepository = playerRepository;
            _playerPool = playerPool;
        }
        
    }
}