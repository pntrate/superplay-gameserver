using Microsoft.Extensions.DependencyInjection;
using SuperPlay.Game.Infrastructure.Abstraction;
using SuperPlay.Game.Infrastructure.Repositories;

namespace SuperPlay.Game.Infrastructure
{
    public static class Bootstrapper
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IPlayerRepository, PlayerRepository>();
        }
    }
}