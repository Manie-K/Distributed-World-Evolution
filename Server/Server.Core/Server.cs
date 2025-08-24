using System.Net.Sockets;
using System.Net;
using Server.Core.Lobby;
using SharedLibrary;
using Server.Core.Logging;
using System.Drawing;

namespace Server.Core
{
    internal class Server
    {
        private readonly LobbyManager lobbyManager;

        public static event Action<OnMessageFromClientEventArgs>? OnMessageFromClientReceived;

        public Server()
        {
            lobbyManager = new LobbyManager();

            lobbyManager.OnLog += OnLog_Delegate;
            Lobby.Lobby.OnLog += OnLog_Delegate;
        }


        public void Start(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            Log("Server started...", LogLevelEnum.Info);

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                //Here we have a few options:

                //1. ThreadPool.QueueUserWorkItem(HandleClientConnection, client);
                //2. Convert it into async method
                Task.Factory.StartNew(() => HandleClientConnection(client), TaskCreationOptions.LongRunning); //3.
                Log("New client joined server.", LogLevelEnum.Info);
                MessageManager.SendMessage(client, new InfoMessage("Welcome to the server!"));
            }
        }

        void HandleClientConnection(TcpClient client)
        {
            MessageBase message = MessageManager.ReceiveMessage(client);

            if (message.MessageType == MessageTypeEnum.CreateLobby)
            {
                CreateLobbyMessage createLobbyMessage = (CreateLobbyMessage)message;

                //TODO: validate message; transfer data from message to lobby
                int lobbyId = lobbyManager.CreateAndInitializeLobby();
                try
                {
                    lobbyManager.AddUserToLobby(lobbyId, client);
                }
                catch (Exception ex)
                {
                    Log(ex.Message, LogLevelEnum.Error);
                    client.Close();
                }
            }

            //Joining existing lobby
            else if (message.MessageType == MessageTypeEnum.JoinLobby)
            {
                JoinLobbyMessage joinLobbyMessage = (JoinLobbyMessage)message;

                try
                {
                    lobbyManager.AddUserToLobby(joinLobbyMessage.LobbyID, client);
                }
                catch (Exception ex)
                {
                    Log(ex.Message, LogLevelEnum.Error);
                    client.Close();
                }
            }

            else if (message.MessageType == MessageTypeEnum.EntityState) //probably other types also
            {
                OnMessageFromClientReceived?.Invoke(new OnMessageFromClientEventArgs(client, message));
            }

            else
            {
                client.Close();
            }

        }

        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            Log(e.Message, e.LogLevel, sender, e.Timestamp);
        }

        private void Log(string message, LogLevelEnum level, object? sender = null, DateTime? timestamp = null)
        {
            timestamp ??= DateTime.Now;

            var color = level switch
            {
                LogLevelEnum.Debug => Color.White,
                LogLevelEnum.Info => Color.Green,
                LogLevelEnum.Warning => Color.Yellow,
                LogLevelEnum.Error => Color.OrangeRed,
                LogLevelEnum.Critical => Color.Red,
                _ => Color.Gray,
            };

            //TODO: Create GUI console with rich text support
            Console.WriteLine($"[{timestamp:HH:mm:ss}] [{level}] {message}");
        }
    }
}
