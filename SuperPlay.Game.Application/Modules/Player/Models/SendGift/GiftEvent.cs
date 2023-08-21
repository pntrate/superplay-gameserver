using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Application.Modules.Player.Models.SendGift
{
    public class GiftEvent
    {
        public Guid SenderId { get; set; }
        public Resource? Resource { get; set; }
    }
}