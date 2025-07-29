using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class MessageManager
    {
        public static IMessage RecvieMessage(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            NetworkStream stream = client.GetStream();

            int count = stream.Read(buffer, 0, buffer.Length);
            string stringMessage = Encoding.UTF8.GetString(buffer, 0, count);

            IMessage message = JsonSerializer.Deserialize<IMessage>(stringMessage);

            return message;
        }

        public static bool SendMessage(IMessage message, TcpClient client)
        {
            try
            {
                if (client == null || !client.Connected) return false;

                NetworkStream stream = client.GetStream();
                string json = message.BuildJson();

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
