using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Application.Modules.Player.Models.SendGift
{
    public class SendGiftRequest
    {
        public Guid FriendPlayerId { get; set; }
        public ResourceType ResourceType { get; set; }
        public int ResourceValue { get; set; }
    }
}