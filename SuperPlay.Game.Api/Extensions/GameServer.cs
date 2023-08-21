using SuperPlay.Game.Application.Modules.Player.Hub;

namespace SuperPlay.Game.Api.Extensions
{
    public static class GameServer
    {
        public static void UseGameServer(this IApplicationBuilder app)
        {
            //register handlers here

            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var channel = new PlayerChannel(webSocket);

                    channel.OnMessage += async (msg) =>
                    {
                        //var res =
                    };

                    await channel.Listen(CancellationToken.None);
                }
                else
                {
                    await next(context);
                }
            });
        }

    }
}
