using Server.Core.Lobby;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using SharedLibrary;

namespace Server.Core
{
    public class Server 
    {
        private static readonly Lazy<Server> _instance = new Lazy<Server>(() => new Server());
        public static Server Instance => _instance.Value;


        public readonly LobbyManager lobbyManager;

        public static event Action<OnMessageFromClientEventArgs>? OnMessageFromClientReceived;
        
        private TcpClient clientUI = null;
        private ConcurrentQueue<LogMessage> logQueue = new ConcurrentQueue<LogMessage>();

        private Server()
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
            //TODO: remove
            lobbyManager.CreateAndInitialiseLobby("TEST", 2, 1, [] );

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

                MessageBase message = MessageManager.ReceiveMessage(client);
                if (message is RoleMessage)
                {
                    RoleMessage roleMessage = (RoleMessage)message;
                    if (roleMessage.Role == RoleEnum.User)
                    {
                        Log("New client joined server - " + roleMessage.Role.ToString(), LogLevelEnum.Info);
                        _ = MessageManager.SendMessageAsync(client, new InfoMessage("Welcome to the server!"));

                        await Task.Factory.StartNew(() => HandleUserConnection(client), TaskCreationOptions.LongRunning);
                    }
                    else
                    {
                        clientUI = client;

                        while (logQueue.TryDequeue(out var log))
                        {
                            _ = MessageManager.SendMessageAsync(clientUI, log);
                        }

                        Log("New client joined server - " + roleMessage.Role.ToString(), LogLevelEnum.Info);
                        Log("Server working...", LogLevelEnum.Info);
                    }
                }
                else
                {
                    //TODO: send error message to client
                    client.Close();
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

                //Creating new lobby
                if (message.MessageType == MessageTypeEnum.CreateLobby)
                {
                    CreateLobbyMessage createLobbyMessage = (CreateLobbyMessage)message;

                    int lobbyId = lobbyManager.CreateAndInitialiseLobby(createLobbyMessage.LobbyName, createLobbyMessage.MaxPlayers,
                        createLobbyMessage.MapID, createLobbyMessage.ModuleIDs);

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

                //Disjoining existing lobby
                else if (message.MessageType == MessageTypeEnum.DisjoinLobby)
                {
                    DisjoinLobbyMessage joinLobbyMessage = (DisjoinLobbyMessage)message;

                    try
                    {
                        lobbyManager.RemoveUserFromLobby(joinLobbyMessage.LobbyID, client);
                        client.Close();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, LogLevelEnum.Error);
                        client.Close();
                    }
                }

                else if (message.MessageType == MessageTypeEnum.GetMessage)
                {
                    GetMessage getMessage = (GetMessage)message;

                    switch (getMessage.GetMessageType)
                    {
                        case GetMessageTypeEnum.GetAllLobbies:
                            //TODO: removed hardcoded lobbies
                            var lobbies = new List<LobbyDTO>();
                            lobbies.Add(new LobbyDTO
                            {
                                LobbyId = 1,
                            });

                            _ = MessageManager.SendMessageAsync(client, new LobbiesMessage(lobbies));
                            break;
                        case GetMessageTypeEnum.GetAllModules:
                            //TODO: removed hardcoded modules
                            var modules = new List<ModuleDTO>();
                            modules.Add(new ModuleDTO
                            (1, "Test Module", "1.0", "Author", new { Description = "This is a test module." }, 
                                new List<BehviourDTO>
                                {
                                    new BehviourDTO (1, "Test Behaviour", "This is a test behaviour.")
                                }
                            ));

                            _ = MessageManager.SendMessageAsync(client, new ModulesMessage(modules));
                            break;

                        default:
                            Log("Unknown GetMessage request: " + getMessage.GetMessageType, LogLevelEnum.Warning);
                            break;
                    }

                }

                else
                {
                    OnMessageFromClientReceived?.Invoke(new OnMessageFromClientEventArgs(client, message));
                }
            }

        }

        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            Task.Run(() => Log(e.Message, e.LogLevel, sender, e.Timestamp));
        }

        private void Log(string message, LogLevelEnum level, object? sender = null, DateTime? timestamp = null)
        {
            var args = new OnLogEventArgs(message, level);

            int senderID = (sender is Lobby.Lobby lobby) ? lobby.LobbyId : -1;
            var logMessage = new LogMessage(args, senderID);
            
            logQueue.Enqueue(logMessage);
            if (clientUI != null && clientUI.Connected)
            {
                _ = MessageManager.SendMessageAsync(clientUI, logMessage);
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
