using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SharedLibrary
{
    public class MessageManager
    {
        public static MessageBase ReceiveMessage(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] lengthBuffer = new byte[4];
            stream.Read(lengthBuffer, 0, 4);
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            byte[] messageBuffer = new byte[messageLength];
            int totalRead = 0;
            while (totalRead < messageLength)
            {
                int read = stream.Read(messageBuffer, totalRead, messageLength - totalRead);
                if (read == 0)
                    throw new IOException("Connection with sender lost.");
                totalRead += read;
            }

            string json = Encoding.UTF8.GetString(messageBuffer);
            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;
            string messageTypeString = root.GetProperty("MessageType")
                                     .GetString();

            MessageBase? message;
            if(Enum.TryParse(messageTypeString, out MessageTypeEnum parsedMessageType))
            {
                message = parsedMessageType switch
                {
                    MessageTypeEnum.CreateLobby => JsonSerializer.Deserialize<CreateLobbyMessage>(json),
                    MessageTypeEnum.UpdateWorldEntityState => JsonSerializer.Deserialize<UpdateWorldEntityStateMessage>(json),
                    MessageTypeEnum.RefreshWorldEntities => JsonSerializer.Deserialize<RefreshWorldEntitiesMessage>(json),
                    MessageTypeEnum.DefaultMessage => JsonSerializer.Deserialize<DefaultMessage>(json),
                    MessageTypeEnum.UpdateUserState => JsonSerializer.Deserialize<UpdateUserStateMessage>(json),
                    MessageTypeEnum.JoinLobby => JsonSerializer.Deserialize<JoinLobbyMessage>(json),
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
               throw new NotSupportedException($"Undefined message type: {messageTypeString}");
            }

            return message ?? throw new Exception("Message null");
        }

        public static bool SendMessage(TcpClient client, MessageBase message)
        {
            try
            {
                if (client == null || !client.Connected) return false;

                string json = message.BuildJson();
                NetworkStream stream = client.GetStream();

                byte[] messageBytes = Encoding.UTF8.GetBytes(json);
                byte[] lengthPrefix = BitConverter.GetBytes(messageBytes.Length);

                stream.Write(lengthPrefix, 0, lengthPrefix.Length);
                stream.Write(messageBytes, 0, messageBytes.Length);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

    }

}
