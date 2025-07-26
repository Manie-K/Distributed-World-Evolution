using System.IO;
using System.Net.Sockets;
using System.Text;
using Server.Shared.Logging;

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

            NetworkStream stream = client.GetStream();

            string msg = $"You have joined lobby {LobbyId}.\n";
            byte[] response = Encoding.UTF8.GetBytes(msg);
            stream.Write(response, 0, response.Length);
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
                        string message = GetMessageFromClient(client);

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

        private string GetMessageFromClient(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            int count = client.GetStream().Read(buffer, 0, buffer.Length);

            string msg = Encoding.UTF8.GetString(buffer, 0, count);
            return msg;
        }

        private void SendMessageToClient(TcpClient client, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
        }
    }
}
