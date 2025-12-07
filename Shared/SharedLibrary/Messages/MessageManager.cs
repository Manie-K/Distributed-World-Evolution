using SharedLibrary.Messages.BehaviourMessages;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Class for managing sending and receiving messages over TCP.
    /// </summary>
    public class MessageManager
    {
        /// <summary>
        /// Invoked when a message is received.
        /// </summary>
        public static event Action<MessageBase?>? MessageReceived;
        /// <summary>
        /// Invoked when a message is sent.
        /// </summary>
        public static event Action<bool>? MessageSended;

        /// <summary>
        /// Receives a message from the specified TCP client asynchronously.
        /// </summary>
        public static async Task<MessageBase> ReceiveMessageAsync(TcpClient client)
        {
            try
            {
                if (client == null || !client.Connected) throw new IOException("Client is not connected.");

                NetworkStream stream = client.GetStream();
                byte[] lengthBuffer = new byte[4];
                await stream.ReadAsync(lengthBuffer, 0, 4);
                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

                byte[] messageBuffer = new byte[messageLength];
                int totalRead = 0;
                while (totalRead < messageLength)
                {
                    int read = await stream.ReadAsync(messageBuffer, totalRead, messageLength - totalRead);
                    if (read == 0)
                    {
                        throw new IOException("Connection with sender lost.");
                    }
                    totalRead += read;
                }

                string json = Encoding.UTF8.GetString(messageBuffer);
                using JsonDocument document = JsonDocument.Parse(json);
                JsonElement root = document.RootElement;
                string? messageTypeString = root.GetProperty("MessageType").GetString();

                MessageBase? message;
                if (Enum.TryParse(messageTypeString, out MessageTypeEnum parsedMessageType))
                {
                    message = parsedMessageType switch
                    {
                        MessageTypeEnum.CreateLobby => JsonSerializer.Deserialize<CreateLobbyMessage>(json),
                        MessageTypeEnum.UserInteraction => JsonSerializer.Deserialize<UserInteractionMessage>(json),
                        MessageTypeEnum.WorldState => JsonSerializer.Deserialize<WorldStateMessage>(json),
                        MessageTypeEnum.InfoMessage => JsonSerializer.Deserialize<InfoMessage>(json),
                        MessageTypeEnum.JoinLobby => JsonSerializer.Deserialize<JoinLobbyMessage>(json),
                        MessageTypeEnum.RoleMessage => JsonSerializer.Deserialize<RoleMessage>(json),
                        MessageTypeEnum.LogMessage => JsonSerializer.Deserialize<LogMessage>(json),
                        MessageTypeEnum.DisjoinLobby => JsonSerializer.Deserialize<DisjoinLobbyMessage>(json),
                        MessageTypeEnum.ModuleList => JsonSerializer.Deserialize<ModuleListMessage>(json),
                        MessageTypeEnum.CreateModule => JsonSerializer.Deserialize<CreateModuleMessage>(json),
                        MessageTypeEnum.LobbyList => JsonSerializer.Deserialize<LobbyListMessage>(json),
                        MessageTypeEnum.GetMessage => JsonSerializer.Deserialize<GetMessage>(json),
                        MessageTypeEnum.LobbyData => JsonSerializer.Deserialize<LobbyDataMessage>(json),
                        MessageTypeEnum.BehaviourList => JsonSerializer.Deserialize<BehaviourListMessage>(json),
                        _ => throw new NotImplementedException(),
                    };
                }
                else
                {
                    throw new NotSupportedException($"Undefined message type: {messageTypeString}");
                }

                MessageReceived?.Invoke(message);
                return message ?? throw new Exception("Message null");
            }
            catch (IOException ex)
            {
                MessageReceived?.Invoke(null);
                throw new IOException("Client is diconnected.", ex);
            }
        }

        /// <summary>
        /// Sends a message to the specified TCP client asynchronously.
        /// </summary>
        public static async Task<bool> SendMessageAsync(TcpClient client, MessageBase message)
        {
            try
            {
                if (client == null || !client.Connected) throw new IOException("Client is not connected.");

                string json = message.BuildJson();
                byte[] messageBytes = Encoding.UTF8.GetBytes(json);
                byte[] lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

                NetworkStream stream = client.GetStream();

                await stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                await stream.FlushAsync();

                MessageSended?.Invoke(true);
                return true;
            }
            catch (IOException ex)
            {
                MessageSended?.Invoke(false);
                throw new IOException("Client is diconnected.", ex);
            }
        }
    }
}
