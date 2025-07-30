using System.IO;
using System.Net.Sockets;
using System.Text;
using Server.Shared.Logging;
using Server.Shared.Messages;

namespace Server.Core.Lobby
{
    internal class Lobby : ILobby
    {
        /// <inheritdoc/>
        public int LobbyId { get; private init; }

        private ILogger logger;
        private List<TcpClient> clients;
        private bool running;

        /// <summary>
        /// Default constructor for Lobby.
        /// <paramref name="id"/> Unique identifier for the lobby.
        /// <paramref name="logger"/> Logger to log about this lobby.
        /// </summary>
        public Lobby(int id, ILogger logger)
        {
            LobbyId = id;
            this.logger = logger;

            clients = new List<TcpClient>();
            running = true;
        }

        public void AddClient(TcpClient client)
        {
            lock (clients)
            {
                clients.Add(client);
            }

            MessageManager.SendMessage(client, new StringMessage($"You have joined lobby {LobbyId}.\n"));
        }

        public void Run()
        {
            logger.Log($"Lobby {LobbyId} started.", LogLevel.Info);

            while (running)
            {
                Update();
                Thread.Sleep(10);
            }

            logger.Log($"Lobby {LobbyId} closed.", LogLevel.Info);
        }

        private void Update()
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    if (client.Available > 0)
                    {
                        //Chat communication between users
                        IMessage message = GetMessageFromClient(client);

                        foreach (var c in clients)
                        {
                            if (c != client)
                            {
                                SendMessageToClient(c, message);
                            }
                        }
                    }
                }
            }
        }

        private IMessage GetMessageFromClient(TcpClient client)
        {
            IMessage message = MessageManager.ReceiveMessage(client);

            return message;
        }

        private void SendMessageToClient(TcpClient client, IMessage message)
        {
            MessageManager.SendMessage(client, message);
        }
    }
}
