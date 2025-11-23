using Server.Core.Lobby;
using Server.Core.Services;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using SharedLibrary.Messages.BehaviourMessages;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;

namespace Server.Core
{
    /// <summary>
    /// Singleton class representing the server
    /// </summary>
    public class Server 
    {
        /// <summary>
        /// Singleton instance of the Server class
        /// </summary>
        public static Server Instance = new Server();

        /// <summary>
        /// Lobby manager instance
        /// </summary>
        private readonly LobbyManager lobbyManager =  new LobbyManager();

        /// <summary>
        /// OnMessageFromClientReceived event
        /// </summary>
        public static event Action<OnMessageFromClientEventArgs>? OnMessageFromClientReceived;

        /// <summary>
        /// Logger service instance
        /// </summary>
        private LoggerService loggerService = new LoggerService();

        /// <summary>
        /// Constructor for the Server class
        /// </summary>
        private Server()
        {
            lobbyManager.OnLog += OnLog_Delegate;
            Lobby.Lobby.OnLog += OnLog_Delegate;
        }

        /// <summary>
        /// Starts the server
        /// </summary>
        public async Task StartAsync(string[] args)
        {
            loggerService.Log("Server started...", LogLevelEnum.Info);
            await StartAcceptingClientsAsync();
        }


        #region Client Handling

        private async Task StartAcceptingClientsAsync()
        {
            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .Build();

            string host = config["TcpSettings:Host"]!;
            int port = int.Parse(config["TcpSettings:Port"]!);

            IPAddress address = host == "0.0.0.0" ? IPAddress.Any : IPAddress.Parse(host);

            TcpListener listener = new TcpListener(address, port);
            listener.Start();

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                _ = HandleClientAsync(client);
            }
        }
        
        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                MessageBase message = await MessageManager.ReceiveMessageAsync(client);

                if (message is not RoleMessage roleMessage)
                {
                    await SendAndCloseAsync(client, new InfoMessage(InfoMessageTypeEnum.Warning, "Unknown client role."));
                    return;
                }

                loggerService.Log($"New client joined server - {roleMessage.Role}", LogLevelEnum.Info);

                _ = HandleClientByRoleAsync(client, roleMessage.Role);
            }
            catch (Exception ex)
            {
                loggerService.Log($"Error while handling client: {ex.Message}", LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Server error. Try again."));
                client.Close();
            }
        }

        private async Task HandleClientByRoleAsync(TcpClient client, RoleEnum role)
        {
            switch (role)
            {
                case RoleEnum.User:
                    await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.ServerConnected, "Welcome to the server!"));
                    _ = HandleUserConnectionAsync(client);
                    break;

                case RoleEnum.UI:
                    loggerService.ClientUI = client;
                    await SafeSendAsync(client, new LobbyListMessage(lobbyManager.GetAllLobbies().Select(l => l.ToDTO()).ToList()));
                    Task.Delay(2000).Wait();
                    _ = loggerService.StartAsync(CancellationToken.None);
                    break;

                default:
                    await SendAndCloseAsync(client, new InfoMessage(InfoMessageTypeEnum.Warning, "Unsupported role."));
                    break;
            }
        }
        #endregion

        #region User Handling
        private async Task HandleUserConnectionAsync(TcpClient client)
        {
            try
            {
                while (true)
                {
                    MessageBase message = await MessageManager.ReceiveMessageAsync(client);
                    _ = HandleMessageAsync(client, message);
                }
            }
            catch (IOException)
            {
                loggerService.Log("Client disconnected.", LogLevelEnum.Info);
                client.Close();
            }
            catch (Exception ex)
            {
                loggerService.Log($"Error while handling client: {ex.Message}", LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Internal server error."));
                client.Close();
            }
        }

        private async Task HandleMessageAsync(TcpClient client, MessageBase message)
        {
            switch (message.MessageType)
            {
                case MessageTypeEnum.CreateLobby:
                    await HandleCreateLobbyAsync(client, (CreateLobbyMessage)message);
                    break;

                case MessageTypeEnum.JoinLobby:
                    await HandleJoinLobbyAsync(client, (JoinLobbyMessage)message);
                    break;

                case MessageTypeEnum.DisjoinLobby:
                    await HandleDisjoinLobbyAsync(client, (DisjoinLobbyMessage)message);
                    break;

                case MessageTypeEnum.GetMessage:
                    await HandleGetMessageAsync(client, (GetMessage)message);
                    break;

                case MessageTypeEnum.CreateModule:
                    await HandleCreateModuleAsync(client, (CreateModuleMessage)message);
                    break;

                default:
                    OnMessageFromClientReceived?.Invoke(new OnMessageFromClientEventArgs(client, message));
                    break;
            }
        }

        private async Task HandleCreateLobbyAsync(TcpClient client, CreateLobbyMessage msg)
        {
            try
            {
                int lobbyID = lobbyManager.CreateAndInitializeLobby(
                    msg.LobbyName, msg.MaxPlayers, msg.MapID, msg.WalkableTiles, msg.FertileTiles, msg.ModuleIDs);

                lobbyManager.AddUserToLobby(lobbyID, client, msg.UserName, out Guid userEntityID);
                Lobby.Lobby lobby = lobbyManager.GetLobby(lobbyID);

                loggerService.SendLobby(lobby);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyCreated, "New lobby created!"));
                await SafeSendAsync(client, new LobbyDataMessage(lobby.ToDTO(), userEntityID));
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyJoined, "Welcome to the new lobby!"));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotCreated, "Lobby creation failed. Error:" + ex));
            }
        }

        private async Task HandleJoinLobbyAsync(TcpClient client, JoinLobbyMessage msg)
        {
            try
            {
                lobbyManager.AddUserToLobby(msg.LobbyID, client, msg.UserName, out Guid userEntityID);
                if (userEntityID == Guid.Empty)
                {
                    await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotJoined, "Lobby is full. Cannot join."));
                    return;
                }
                Lobby.Lobby lobby = lobbyManager.GetLobby(msg.LobbyID);

                await SafeSendAsync(client, new LobbyDataMessage(lobby.ToDTO(), userEntityID));
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyJoined, "Welcome to lobby!"));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotJoined, "Lobby join failed. Error:" + ex));
            }
        }

        private async Task HandleDisjoinLobbyAsync(TcpClient client, DisjoinLobbyMessage msg)
        {
            try
            {
                lobbyManager.RemoveUserFromLobby(msg.LobbyID, client);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyDisjoined, "See you soon!"));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotDisjoined, "Error leaving lobby."));
            }
        }

        private async Task HandleGetMessageAsync(TcpClient client, GetMessage msg)
        {
            try
            {
                switch (msg.GetMessageType)
                {
                    case GetMessageTypeEnum.LobbyList:
                        await SafeSendAsync(client, new LobbyListMessage(lobbyManager.GetAllLobbies().Select(l => l.ToDTO()).ToList()));
                        break;

                    case GetMessageTypeEnum.ModuleList:
                        var modules = ModuleService.Instance.GetAllModules().Select(m => m.ToDTO()).ToList();
                        await SafeSendAsync(client, new ModuleListMessage(modules));
                        break;

                    case GetMessageTypeEnum.BehaviourList:
                        var behaviours = BehaviourService.Instance.GetAllBehaviours().Select(b => b.ToDTO()).ToList();
                        await SafeSendAsync(client, new BehaviourListMessage(behaviours));
                        break;

                    default:
                        await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Unknown GetMessage Type."));
                        break;
                }
            }
            catch (Exception ex)
            {
               loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Error while fetching data. Error:" + ex));
            }
        }

        private async Task HandleCreateModuleAsync(TcpClient client, CreateModuleMessage msg)
        {
            try
            {
                ModuleService.Instance.CreateModule(msg.ModuleDTO);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.ModuleCreated, "Module created successfully."));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.ModuleNotCreated, "Module creation failed. Error:" + ex));
            }
        }
        #endregion

        #region Message Sending Helpers
        private async Task SendAndCloseAsync(TcpClient client, InfoMessage message)
        {
            await SafeSendAsync(client, message);
            client.Close();
        }

        private async Task SafeSendAsync(TcpClient client, MessageBase message)
        {
            try
            {
                await MessageManager.SendMessageAsync(client, message);
            }
            catch (Exception ex)
            {
                loggerService.Log($"Failed to send message to client: {ex.Message}", LogLevelEnum.Error);
            }
        }
        #endregion

        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            loggerService.Log(e.Message, e.LogLevel, sender);
        }

    }
}
