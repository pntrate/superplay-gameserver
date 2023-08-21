using Serilog;
using SuperPlay.Game.Application.Modules.Player.Models.Common;
using SuperPlay.Game.Application.Modules.Player.Models.Login;
using SuperPlay.Game.Application.Modules.Player.Models.SendGift;
using SuperPlay.Game.Application.Modules.Player.Models.UpdateResources;
using SuperPlay.Game.Domain.Models.Resources;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SuperPlay.Game.Client
{
    public class Client
    {
        private readonly string _serverUrl;
        private ClientWebSocket _clientWebSocket;
        private ILogger _logger;
        private Task? _listenTask;
        private Guid _playerId;

        public event Action<LoginResponse>? OnLoggedIn;
        public event Action<UpdateResourcesResponse>? OnResourcesUpdated;
        public event Action<SendGiftResponse>? OnGiftSent;
        public event Action<GiftEvent>? OnGiftEvent;
        public event Action<ErrorMessage>? OnError;

        public Client(string serverUrl, ILogger logger)
        {
            _serverUrl = serverUrl;
            _logger = logger;
            _clientWebSocket = new ClientWebSocket();
        }

        public async Task Connect(Guid deviceId)
        {
            await _clientWebSocket.ConnectAsync(new Uri(_serverUrl), CancellationToken.None);
            _listenTask = Task.Factory.StartNew(ListenForMessages);
        }

        public async Task Login(Guid deviceId)
        {
            await Send(new LoginRequest
            {
                DeviceId = deviceId
            }, CancellationToken.None);
        }

        public async Task UpdateResources(ResourceType resourceType, int value)
        {
            await Send(new UpdateResourcesRequest
            {
                ResourceType = resourceType,
                ResourceValue = value
            }, CancellationToken.None);
        }

        public async Task SendGift(Guid friendId, ResourceType resourceType, int value)
        {
            await Send(new SendGiftRequest
            {
                FriendPlayerId = friendId,
                ResourceType = resourceType,
                ResourceValue = value
            }, CancellationToken.None);
        }

        private async Task Send<T>(T req, CancellationToken cancellationToken)
        {
            var messageType = typeof(T)?.GetCustomAttribute<MessageTypeAttribute>()?.Name;

            if (messageType == null)
            {
                throw new ArgumentException($"{nameof(messageType)} has no defined message type attribute");
            }

            var message = new Message
            {
                Type = messageType,
                Content = JsonSerializer.Serialize(req),
                PlayerId = _playerId
            };
            var sendBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await _clientWebSocket.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, cancellationToken);
        }

        public async Task ListenForMessages()
        {
            var buffer = new byte[4 * 1024];
            try
            {
                while (true)
                {
                    var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed connection", CancellationToken.None);
                        break;
                    }

                    var messageString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    var message = Parse<Message>(messageString);

                    switch (message.Type)
                    {
                        case "LoginResponse":
                            {
                                var payload = Parse<LoginResponse>(message.Content);
                                _playerId = payload.PlayerId;
                                OnLoggedIn?.Invoke(payload);
                                break;
                            }
                        case "UpdateResourcesResponse":
                            {
                                var payload = Parse<UpdateResourcesResponse>(message.Content);
                                OnResourcesUpdated?.Invoke(payload);
                                break;
                            }

                        case "SendGiftResponse":
                            {
                                var payload = Parse<SendGiftResponse>(message.Content);
                                OnGiftSent?.Invoke(payload);
                                break;
                            }
                        case "GiftEvent":
                            {
                                var payload = Parse<GiftEvent>(message.Content);

                                OnGiftEvent?.Invoke(payload);
                                break;
                            }
                        case "ErrorMessage":
                            {
                                var payload = Parse<ErrorMessage>(message.Content);

                                OnError?.Invoke(payload);
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception: {ex}");
                throw;
            }
        }

        private T Parse<T>(string data)
        {
            var res = JsonSerializer.Deserialize<T>(data);
            return res == null ? throw new Exception("Response cannot be parsed") : res;
        }

    }
}
