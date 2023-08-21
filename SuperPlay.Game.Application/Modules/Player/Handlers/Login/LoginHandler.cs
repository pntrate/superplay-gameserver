using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.Login;
using SuperPlay.Game.Infrastructure.Abstraction;

namespace SuperPlay.Game.Application.Modules.Player.Handlers.Login
{
    public class LoginHandler : HandlerBase<LoginRequest, LoginResponse>
    {
        public LoginHandler(IPlayerRepository playerRepository, IPlayerPool playerPool) : base(playerRepository, playerPool)
        {

        }

        public override Task<LoginResponse> Handle(LoginRequest request, PlayerContext context)
        {
            var player = _playerRepository.GetByDeviceId(request.DeviceId);
            if (player == null)
            {
                player = Domain.Models.Players.Player.Create(request.DeviceId);
                _playerRepository.Create(player);
            }

            player.Login();
            _playerPool.PlayerLoggedIn(player.Id, context.PlayerChannel!);

            return Task.FromResult(new LoginResponse
            {
                PlayerId = player.Id
            });
        }
    }
}