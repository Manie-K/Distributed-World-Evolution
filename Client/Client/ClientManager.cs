using SharedLibrary;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public class ClientManager
    {
        public const double CLIENT_UPDATES_PER_SECOND = 64;
        public TcpClient Client { get; private set; }

        private readonly object entitiesLock = new object();
        private Dictionary<Guid, WorldEntityDTO> entities = new Dictionary<Guid, WorldEntityDTO>();
        public IReadOnlyDictionary<Guid, WorldEntityDTO> Entities
        {
            get
            {
                lock (entitiesLock)
                {
                    return entities;
                }
            }
        }

        private string serverIp;
        private int port;
        private Thread receiveThread;

        public ClientManager()
        {
            serverIp = "127.0.0.1";
            port = 5000;
            StartClient();
        }

        private void StartClient()
        {
            try
            {
                Client = new TcpClient(serverIp, port);
                _ = MessageManager.SendMessageAsync(Client, new RoleMessage(RoleEnum.User));
                receiveThread = new Thread(ReceiveMessages)
                {
                    IsBackground = true
                };
                receiveThread.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Client] error: {e.Message}");
            }
        }

        public void CloseClient()
        {
            Client?.Close();
            receiveThread?.Join();
        }

        private void ReceiveMessages()
        {
            while (Client.Connected)
            {
                MessageBase message = MessageManager.ReceiveMessage(Client);

                if (message == null)
                {
                    Console.WriteLine("Received null message");
                }
                else if (message.MessageType == MessageTypeEnum.InfoMessage)
                {
                    InfoMessage infoMessage = (InfoMessage)message;
                    Console.WriteLine($"[Client] Received message: {infoMessage.MessageContent}");
                }
                else if (message.MessageType == MessageTypeEnum.WorldState)
                {
                    WorldStateMessage worldStateMessage = (WorldStateMessage)message;
                    lock (entitiesLock)
                    {
                        entities = worldStateMessage.Entities.ToDictionary(e => e.Id);
                    }
                }
                else
                {
                    Console.WriteLine("Received unknown message");
                }
            }
        }
    }
}
