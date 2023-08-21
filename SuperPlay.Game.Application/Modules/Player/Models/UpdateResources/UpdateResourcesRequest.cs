using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Application.Modules.Player.Models.UpdateResources
{
    [MessageType("UpdateResourcesRequest")]
    public class UpdateResourcesRequest
    {
        public ResourceType ResourceType { get; set; }
        public int ResourceValue { get; set; }
    }
}