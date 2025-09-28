using Server.Core.Lobby;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using SharedLibrary.DTOs.ModuleDTO;
using SharedLibrary.DTOs.LobbyDTO;

namespace Server.Core
{
    public class Server 
    {
        private static readonly Lazy<Server> _instance = new Lazy<Server>(() => new Server());
        public static Server Instance => _instance.Value;

        public readonly LobbyManager lobbyManager;

        public static event Action<OnMessageFromClientEventArgs>? OnMessageFromClientReceived;

        private TcpClient clientUI;

        private ConcurrentQueue<LogMessage> logQueue = new ConcurrentQueue<LogMessage>();

        private Server()
        {
            lobbyManager = new LobbyManager();

            lobbyManager.OnLog += OnLog_Delegate;
            Lobby.Lobby.OnLog += OnLog_Delegate;
        }

        public void Start(string[] args)
        {
            //TODO: change to config
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            ////
            listener.Start();

            Log("Server started...", LogLevelEnum.Info);
            //TODO: remove hardcoded lobby
            lobbyManager.CreateAndInitialiseLobby("TEST", 2, 1, [] );
            ////

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
                MessageBase message = await MessageManager.ReceiveMessageAsync(client);
                if (message is RoleMessage)
                {
                    RoleMessage roleMessage = (RoleMessage)message;
                    if (roleMessage.Role == RoleEnum.User)
                    {
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.ServerConnected ,"Welcome to the server!"));
                    }
                    else
                    {
                        clientUI = client;

                        while (logQueue.TryDequeue(out var log))
                        {
                            _ = MessageManager.SendMessageAsync(clientUI, log);
                        }

                        Log("Server working...", LogLevelEnum.Info);
                    }

                    Log("New client joined server - " + roleMessage.Role.ToString(), LogLevelEnum.Info);

                    await HandleUserConnectionAsync(client);
                }
                else
                {
                    await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.Warning , "Unknown client role."));
                    client.Close();
                }

            }
            catch (Exception ex)
            {
                await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Server error. Try again."));
                Log(ex.Message, LogLevelEnum.Error);
                client.Close();
            }
        }

        private async Task HandleUserConnectionAsync(TcpClient client)
        {
            while (true)
            {
                MessageBase message = await MessageManager.ReceiveMessageAsync(client);

                //Creating new lobby
                if (message.MessageType == MessageTypeEnum.CreateLobby)
                {
                    CreateLobbyMessage createLobbyMessage = (CreateLobbyMessage)message;
                    int lobbyID = lobbyManager.CreateAndInitialiseLobby(createLobbyMessage.LobbyName, createLobbyMessage.MaxPlayers,
                        createLobbyMessage.MapID, createLobbyMessage.ModuleIDs);

                    try
                    {
                        lobbyManager.AddUserToLobby(lobbyID, client);

                        //TODO: send full lobby info
                        await MessageManager.SendMessageAsync(client, new LobbyMessage(null, lobbyID));
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyJoined, "Welcome to lobby!"));
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, LogLevelEnum.Error);
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotJoined, "Lobby error. Try again."));
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
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyJoined, "Welcome to lobby!"));
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, LogLevelEnum.Error);
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotJoined, "Lobby error. Try again."));
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
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyDisjoined, "See you soon!"));
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message, LogLevelEnum.Error);
                        await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotDisjoined, "Lobby error. Try again."));
                        client.Close();
                    }
                }

                //Get messages
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
                                ID = 1,
                                Name = "Test Lobby",
                                MaxPlayers = 10,
                            });
                            ////

                            try
                            {
                                await MessageManager.SendMessageAsync(client, new LobbyListMessage(lobbies));
                            }
                            catch (Exception ex)
                            {
                                Log(ex.Message, LogLevelEnum.Error);
                                await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Lobby list error. Try again."));
                                client.Close();
                            }
                            break;

                        case GetMessageTypeEnum.GetAllModules:
                            //TODO: removed hardcoded modules
                            var modules = new List<ModuleDTO>();
                            modules.Add(new ModuleDTO
                            (1, "Test Module", true, 10, 10, 10, new List<BehviourDTO>{
                                    new BehviourDTO (1, "This is a test behaviour.", EntityTypeEnum.Animal)
                                },
                                EntityTypeEnum.Animal
                            ));
                            ////
                            ///
                            try
                            {
                                await MessageManager.SendMessageAsync(client, new ModuleListMessage(modules));
                            }
                            catch (Exception ex)
                            {
                                Log(ex.Message, LogLevelEnum.Error);
                                await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Module list error. Try again."));
                                client.Close();
                            }
                            break;

                        default:
                            await MessageManager.SendMessageAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Unknown GetMessage Type."));
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

            Console.WriteLine($"[{timestamp:HH:mm:ss}] [{level}] {message}");
        }

    }
}
