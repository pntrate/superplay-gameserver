﻿using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.SendGift;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers.SendGift
{
    public class SendGiftHandler : HandlerBase<SendGiftRequest, SendGiftResponse>
    {
        public SendGiftHandler(IPlayerRepository playerRepository, IPlayerPool playerPool) : base(playerRepository, playerPool)
        {

        }

        public override async Task<SendGiftResponse> Handle(SendGiftRequest request, PlayerContext context)
        {
            var sender = _playerRepository.GetById(context.PlayerId) ?? throw new Exception("");
            var friend = _playerRepository.GetById(request.FriendPlayerId) ?? throw new Exception("");

            sender.SendGift(request.ResourceType, request.ResourceValue);
            var resource = friend.UpdateResources(request.ResourceType, request.ResourceValue);

            if (friend.IsOnline)
            {
                await _playerPool.SendMessageTo(friend.Id, resource);
            }

            return new SendGiftResponse();
        }
    }
}