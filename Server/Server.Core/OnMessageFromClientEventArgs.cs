using System.Net.Sockets;
using SharedLibrary;

namespace Server.Core
{
    public class OnMessageFromClientEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public MessageBase Message { get; }
        public OnMessageFromClientEventArgs(TcpClient client, MessageBase message)
        {
            Client = client;
            Message = message;
        }
    }
}