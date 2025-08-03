using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Server.Shared.Logging;

namespace Server.Shared.Messages
{
    public class MessageManager
    {
        public static IMessage ReceiveMessage(TcpClient client)
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
            string messageType = root.GetProperty("MessageType").GetString();

            IMessage message = messageType switch
            {
                "CreateLobby" => JsonSerializer.Deserialize<CreateLobbyMessage>(json),
                "ChangeWorldElementState" => JsonSerializer.Deserialize<ChangeWorldElementStateMessage>(json),
                "String" => JsonSerializer.Deserialize<DefaultMessage>(json),
                "ChangeUserState" => JsonSerializer.Deserialize<ChangeUserStateMessage>(json),
                "JoinLobby" => JsonSerializer.Deserialize<JoinLobbyMessage>(json),
                _ => throw new NotSupportedException($"Undefined message type: {messageType}")
            };

            return message;
        }

        public static bool SendMessage(TcpClient client, IMessage message)
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
