﻿using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.UpdateResources;
using SuperPlay.Game.Domain.Common.Exceptions;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers.UpdateResources
{
    public class UpdateResourcesHandler : MessageHandlerBase<UpdateResourcesRequest, UpdateResourcesResponse>
    {
        public UpdateResourcesHandler(IPlayerRepository playerRepository, IPlayerPool playerPool) : base(playerRepository, playerPool)
        {
            
        }

        public override Task<UpdateResourcesResponse> Handle(UpdateResourcesRequest request, OperationContext context)
        {
            var player = _playerRepository.GetById(context.PlayerId)
                        ?? throw new EntityNotFoundException("Player not found");

            var resource = player.UpdateResources(request.ResourceType, request.ResourceValue);

            return Task.FromResult(new UpdateResourcesResponse
            {
                NewBalance = resource.Balance
            });

        }
    }
}