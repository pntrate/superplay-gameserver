using SuperPlay.Game.Application.Modules.Player.Models.Common;

namespace SuperPlay.Game.Application.Modules.Player.Models.Login
{
    [MessageType("LoginResponse")]
    public class LoginResponse
    {
        public Guid PlayerId { get; set; }
    }
}