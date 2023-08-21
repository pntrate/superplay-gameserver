using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Application.Modules.Player.Models.SendGift
{
    [MessageType("GiftEvent")]
    public class GiftEvent
    {
        public Guid SenderId { get; set; }
        public Resource? Resource { get; set; }
    }
}