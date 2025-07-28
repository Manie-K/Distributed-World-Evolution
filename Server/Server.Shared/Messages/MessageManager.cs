using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class MessageManager
    {
        static object RecvieMessage(IMessageBuilder message, TcpClient client)
        {
            byte[] buffer = new byte[1024];
            NetworkStream stream = client.GetStream();

            int count = stream.Read(buffer, 0, buffer.Length);
            string msg = Encoding.UTF8.GetString(buffer, 0, count);

            return msg;
        }

        static bool SendMessage(IMessageBuilder message, TcpClient client)
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
