using Microsoft.Extensions.DependencyInjection;
using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Hub;

namespace SuperPlay.Game.Application
{
    public static class Bootstrapper
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddSingleton<IPlayerPool, PlayerPool>();
        }
    }
}