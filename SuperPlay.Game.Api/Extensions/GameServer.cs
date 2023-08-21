using SuperPlay.Game.Application.Modules.Player.Abstraction;
using SuperPlay.Game.Application.Modules.Player.Handlers.Login;
using SuperPlay.Game.Application.Modules.Player.Handlers.SendGift;
using SuperPlay.Game.Application.Modules.Player.Handlers.UpdateResources;
using SuperPlay.Game.Application.Modules.Player.Hub;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.Login;
using SuperPlay.Game.Application.Modules.Player.Models.SendGift;
using SuperPlay.Game.Application.Modules.Player.Models.UpdateResources;
using System.Reflection;
using System.Text.Json;

namespace SuperPlay.Game.Api.Extensions
{
    public static class GameServer
    {
        record HandlerMetadata(Type RequestType, Type HandlerType);

        private static Dictionary<string, HandlerMetadata> _handlers = new();

        public static void RegisterMessageHandlers(this IServiceCollection services)
        {
            services.RegisterMessageHandler<LoginRequest, LoginResponse, LoginHandler>();
            services.RegisterMessageHandler<UpdateResourcesRequest, UpdateResourcesResponse, UpdateResourcesHandler>();
            services.RegisterMessageHandler<SendGiftRequest, SendGiftResponse, SendGiftHandler>();
        }

        public static void RegisterMessageHandler<TRequest, TResponse, THandler>(this IServiceCollection services)
            where THandler : class, IMessageHandler<TRequest, TResponse>
        {
            var messageType = typeof(TRequest).GetCustomAttribute<MessageTypeAttribute>()?.Name
                              ?? throw new Exception();

            _handlers.Add(messageType, new HandlerMetadata(typeof(TRequest), typeof(IMessageHandler<TRequest, TResponse>)));
            services.AddScoped<IMessageHandler<TRequest, TResponse>, THandler>();
        }

        public static void KickOffGameServer(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
                {
                    using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    var channel = new PlayerChannel(webSocket);

                    channel.OnMessage += async (message) =>
                    {
                        var result = await ExecuteHandler(channel, message, context.RequestServices);
                        await channel.SendMessage(result);
                    };

                    await channel.Listen(CancellationToken.None);
                }
                else
                {
                    await next(context);
                }
            });
        }

        private static async Task<object?> ExecuteHandler(PlayerChannel channel, Message? message, IServiceProvider serviceProvider)
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(GameServer));

            if (message is null)
            {
                logger.LogError($"Message cannot be null");
                return null;
            }

            var operationContext = new OperationContext()
            {
                PlayerId = message.PlayerId,
                PlayerChannel = channel
            };

            var handlerFound = _handlers.TryGetValue(message.Type, out var handlerMetadata);
            if (!handlerFound)
            {
                logger.LogError($"Operation handler not found for message of type ${message.Type}");
                return null;
            }

            var handlerType = handlerMetadata!.HandlerType;
            var handler = serviceProvider.GetService(handlerType) as IMessageHandler;
            if (handler is null)
            {
                logger.LogError($"Operation handler not found for message of type ${message.Type}");
                return null;
            }

            try
            {
                var request = JsonSerializer.Deserialize(message.Content, handlerMetadata.RequestType);
                if (request is null)
                {
                    logger.LogError($"Error while deserializing message of type ${message.Type}");
                    return null;
                }
                return await handler.Handle(request, operationContext);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while performing operation");
                return null;
            }

        }

    }
}