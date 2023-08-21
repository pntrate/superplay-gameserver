using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.UpdateResources;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers.UpdateResources
{
    public class UpdateResourcesHandler : HandlerBase<UpdateResourcesRequest, UpdateResourcesResponse>
    {
        public UpdateResourcesHandler(IPlayerRepository playerRepository, IPlayerPool playerPool) : base(playerRepository, playerPool)
        {
            
        }

        public override Task<UpdateResourcesResponse> Handle(UpdateResourcesRequest request, PlayerContext context)
        {
            var player = _playerRepository.GetById(context.PlayerId)
                        ?? throw new Exception("");

            var balance = player.UpdateResources(request.ResourceType, request.ResourceValue);

            return Task.FromResult(new UpdateResourcesResponse
            {
                NewBalance = balance
            });

        }
    }
}