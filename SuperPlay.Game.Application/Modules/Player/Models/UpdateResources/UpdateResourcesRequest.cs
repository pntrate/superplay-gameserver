using SuperPlay.Game.Domain.Models.Resources;

namespace SuperPlay.Game.Application.Modules.Player.Models.UpdateResources
{
    public class UpdateResourcesRequest
    {
        public ResourceType ResourceType { get; set; }
        public int ResourceValue { get; set; }
    }
}