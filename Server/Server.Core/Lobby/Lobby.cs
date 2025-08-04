using System.IO;
using System.Net.Sockets;
using System.Text;
using Server.Core.Frames;
using Server.Shared.Logging;
using Server.Shared.Messages;

namespace Server.Core.Lobby
{
    internal class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
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

            MessageManager.SendMessage(client, new DefaultMessage($"You have joined lobby {LobbyId}.\n"));
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
                        MessageBase message = GetMessageFromClient(client);

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

        private MessageBase GetMessageFromClient(TcpClient client)
        {
            MessageBase message = MessageManager.ReceiveMessage(client);

            return message;
        }

        private void SendMessageToClient(TcpClient client, MessageBase message)
        {
            MessageManager.SendMessage(client, message);
        }

        private void SendFrameToClient(TcpClient client, DataFrameBase frame)
        {
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(ms);

        }

        private void SendFrameToAllClients(DataFrameBase frame)
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    SendFrameToClient(client, frame);
                }
            }
        }
    }
}
