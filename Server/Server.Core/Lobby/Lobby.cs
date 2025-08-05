using System.IO;
using System.Net.Sockets;
using System.Text;
using Server.Core.Frames;
using Server.Core.Logging;
using Server.Shared.Messages;

namespace Server.Core.Lobby
{
    internal class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
        /// <inheritdoc/>
        public int LobbyId { get; private init; }

        private List<TcpClient> clients;
        private bool running;

        public static event EventHandler<OnLogEventArgs>? OnLog;

        /// <summary>
        /// Default constructor for Lobby.
        /// <paramref name="id"/> Unique identifier for the lobby.
        /// </summary>
        public Lobby(int id)
        {
            LobbyId = id;

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
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            while (running)
            {
                Update();
                Thread.Sleep(10);
            }

            Log($"Lobby {LobbyId} closed.", LogLevelEnum.Info);
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

        private void Log(Exception ex, LogLevelEnum level)
        {
            Log(ex.Message, level);
        }

        private void Log(string message, LogLevelEnum level)
        {
            OnLogEventArgs args = new OnLogEventArgs
            (message, level);

            OnLog?.Invoke(this, args);
        }
    }
}
