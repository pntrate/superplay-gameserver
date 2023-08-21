using SuperPlay.Game.Application.Modules.Player.Models.Common;

namespace SuperPlay.Game.Application.Modules.Player.Models.UpdateResources
{
    [MessageType("UpdateResourcesResponse")]
    public class UpdateResourcesResponse
    {
        public int NewBalance { get; set; }
    }
}