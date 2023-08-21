using SuperPlay.Game.Api.Extensions;
using SuperPlay.Game.Application;
using SuperPlay.Game.Infrastructure;

namespace SuperPlay.Game.Api
{
    public sealed class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayer();
            services.AddInfrastructure();
            services.RegisterMessageHandlers();
        }
    }
}