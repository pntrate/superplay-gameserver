using SuperPlay.Game.Application.Modules.Player.Models.Common;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace SuperPlay.Game.Application.Modules.Player.Hub
{
    public class PlayerChannel
    {
        public event Action<Message?>? OnMessage;
        public event Action? OnClose;

        private readonly WebSocket _webSocket;

        public PlayerChannel(WebSocket webSocket)
        {
            _webSocket = webSocket;
        }

        public async Task SendMessage<T>(T message)
        {
            var sendBuffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new Message
            {
                Content = JsonSerializer.Serialize(message)
            }));

            await _webSocket.SendAsync(new ArraySegment<byte>(sendBuffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public async Task Listen(CancellationToken cancellationToken)
        {
            var result = await ReadMessageByChunks(_webSocket, cancellationToken);

            while (!result.ReceiveResult.CloseStatus.HasValue)
            {
                var messageString = Encoding.UTF8.GetString(result.Data, 0, result.Data.Length);
                var message = JsonSerializer.Deserialize<Message>(messageString);

                OnMessage?.Invoke(message);

                result = await ReadMessageByChunks(_webSocket, cancellationToken);
            }

            OnClose?.Invoke();

            await _webSocket.CloseAsync(
                result.ReceiveResult.CloseStatus!.Value,
                result.ReceiveResult.CloseStatusDescription,
                cancellationToken);
        }

        private async Task<(WebSocketReceiveResult ReceiveResult, byte[] Data)> ReadMessageByChunks(WebSocket webSocket, CancellationToken cancellationToken)
        {
            const int MESSAGE_MAX_SIZE = 1024 * 16;
            const int CHUNK_SIZE = 1024;

            var framePayload = new ArraySegment<byte>(new byte[MESSAGE_MAX_SIZE]);
            var buffer = new ArraySegment<byte>(new byte[CHUNK_SIZE]);

            WebSocketReceiveResult receiveResult;
            var framePayloadSize = 0;

            do
            {
                receiveResult = await _webSocket.ReceiveAsync(buffer, cancellationToken);

                var data = buffer.Slice(0, receiveResult.Count);
                data.CopyTo(framePayload.Slice(framePayloadSize, receiveResult.Count));
                framePayloadSize += receiveResult.Count;
            } while (!receiveResult.EndOfMessage);

            return (receiveResult, framePayload[0..framePayloadSize].ToArray());
        }
    }
}
