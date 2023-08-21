using SuperPlay.Game.Application.Modules.Player.Models.Common;

namespace SuperPlay.Game.Application.Modules.Player.Models.Login
{
    [MessageType("LoginRequest")]
    public class LoginRequest
    {
        public Guid DeviceId { get; set; }
    }
}