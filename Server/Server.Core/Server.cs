using Server.Core.Lobby;
using SharedLibrary;
using SharedLibrary.Messages;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;

namespace Server.Core
{
    public class Server
    {
        private readonly LobbyManager lobbyManager;

        public static event Action<OnMessageFromClientEventArgs>? OnMessageFromClientReceived;
        public TcpClient clientUI;

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

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                _ = WaitForRoleMessageAsync(client);
            }
        }

        private async Task WaitForRoleMessageAsync(TcpClient client)
        {
            try
            {
                while (true)
                {
                    MessageBase message = await Task.Run(() => MessageManager.ReceiveMessage(client));
                    if (message is RoleMessage)
                    {
                        RoleMessage roleMessage = (RoleMessage)message;
                        if (roleMessage.Role == RoleEnum.User)
                        {
                            Log("New client joined server - " + roleMessage.Role.ToString(), LogLevelEnum.Info);
                            _ = MessageManager.SendMessageAsync(client, new InfoMessage("Welcome to the server!"));

                            await Task.Factory.StartNew(() => HandleUserConnection(client), TaskCreationOptions.LongRunning);
                            break;
                        }
                        else
                        {
                            clientUI = client;
                            Log("Server working...", LogLevelEnum.Info);
                            Log("New client joined server - " + roleMessage.Role.ToString(), LogLevelEnum.Info);
                            break;
                        }
                    }
                    else
                    {
                        //TODO: send error message to client
                        client.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message, LogLevelEnum.Error);
                client.Close();
            }
        }

        void HandleUserConnection(TcpClient client)
        {
            while (true)
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
                    break;
                }
            }

        }

        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            Task.Run(() => Log(e.Message, e.LogLevel, sender, e.Timestamp));
        }

        private void Log(string message, LogLevelEnum level, object? sender = null, DateTime? timestamp = null)
        {
            if (sender is Lobby.Lobby lobby)
            {
                int senderID = lobby.LobbyId;
                _ = MessageManager.SendMessageAsync(clientUI, new LogMessage(new OnLogEventArgs(message, level), senderID));
            }
            else
            {
                _ = MessageManager.SendMessageAsync(clientUI, new LogMessage(new OnLogEventArgs(message, level), 0));
            }

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
