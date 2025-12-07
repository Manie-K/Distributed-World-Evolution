using Server.Core.Lobby;
using Server.Core.Services;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using SharedLibrary.Messages.BehaviourMessages;
using System.Net;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using SharedLibrary.DTOs.LobbyDTO;

namespace Server.Core.Connection
{
    /// <summary>
    /// Signleton class for managing client connections and message routing.
    /// </summary>
    public class ConnectionManager : IConnectionManager
    {
        private readonly LobbyManager lobbyManager;

        /// <summary>
        /// Logger service instance.
        /// </summary>
        private readonly LoggerService loggerService;

        /// <summary>
        /// Constructor for ConnectionManager.
        /// </summary>
        public ConnectionManager(LoggerService loggerService, LobbyManager lobbyManager)
        {
            this.loggerService = loggerService;
            this.lobbyManager = lobbyManager;

            lobbyManager.OnLog += OnLog_Delegate;
            Lobby.Lobby.OnLog += OnLog_Delegate;
        }

        #region Client Handling

        /// <inheritdoc/>
        public async Task StartAsync(string[] args)
        {
            loggerService.Log("Server started...", LogLevelEnum.Info);
            await StartAcceptingClientsAsync();
        }

        /// <summary>
        /// Starts accepting client connections asynchronously.
        /// </summary>
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

        /// <summary>
        /// Handles a connected client.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
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

        /// <summary>
        /// Handles the client based on its role.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="role"> The role of the client. </param>
        private async Task HandleClientByRoleAsync(TcpClient client, RoleEnum role)
        {
            switch (role)
            {
                case RoleEnum.User:
                    await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.ServerConnected, "Welcome to the server!"));
                    _ = HandleUserConnectionAsync(client);
                    break;

                case RoleEnum.UI:
                    loggerService.AddClient(client);
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

        /// <summary>
        /// Handles the connection for a user client.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
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
            }
            catch (Exception ex)
            {
                loggerService.Log($"Error while handling client: {ex.Message}", LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Unexpected server error."));
            }

            lobbyManager.RemoveUserFromLobbies(client);
            client.Close();
        }

        /// <summary>
        /// Delegates message handling based on message type.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="message"> The received message. </param>
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
                    lobbyManager.SendMessageToLobbyWithClient(client, message);
                    break;
            }
        }

        /// <summary>
        /// Creates a new lobby and adds the user to it.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="msg"> The create lobby message. </param>
        private async Task HandleCreateLobbyAsync(TcpClient client, CreateLobbyMessage msg)
        {
            try
            {
                int lobbyID = lobbyManager.CreateAndInitializeLobby(
                    msg.LobbyName, msg.MaxPlayers, msg.MapID, msg.WalkableTiles, msg.FertileTiles, msg.ModuleIDs);

                Lobby.Lobby lobby = lobbyManager.GetLobby(lobbyID);
                loggerService.SendLobbyDTO(lobby.ToDTO());

                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyCreated, "New lobby created!"));
                await HandleJoinLobbyAsync(client, new JoinLobbyMessage(lobbyID, msg.UserName));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotCreated, "Lobby creation failed. Unexpected error."));
            }
        }

        /// <summary>
        /// Adds a user to an existing lobby.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="msg"> The join lobby message. </param>
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
                loggerService.SendLobbyDTO(lobby.ToDTO());

                await SafeSendAsync(client, new LobbyDataMessage(lobby.ToDTO(), userEntityID));
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyJoined, "Welcome to lobby!"));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotJoined, "Lobby join failed. Unexpected error."));
            }
        }

        /// <summary>
        /// Removes a user from a lobby.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="msg"> The disjoin lobby message. </param>
        private async Task HandleDisjoinLobbyAsync(TcpClient client, DisjoinLobbyMessage msg)
        {
            try
            {
                Lobby.Lobby lobby = lobbyManager.GetLobby(msg.LobbyID);
                LobbyDTO lobbyDTO = lobby.ToDTO();

                lobbyManager.RemoveUserFromLobby(msg.LobbyID, client);
                loggerService.SendLobbyDTO(lobbyDTO);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyDisjoined, "See you soon!"));
            }
            catch (Exception ex)
            {
                loggerService.Log(ex.Message, LogLevelEnum.Error);
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.LobbyNotDisjoined, "Lobby disjoin failed. Unexpected error"));
            }
        }

        /// <summary>
        /// Retrieves requested data based on GetMessage type.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="msg"> The get message. </param>
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
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.Error, "Error while fetching data. Unexpected error."));
            }
        }

        /// <summary>
        /// Creates a new module based on the provided DTO.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="msg"> The create module message. </param>
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
                await SafeSendAsync(client, new InfoMessage(InfoMessageTypeEnum.ModuleNotCreated, "Module creation failed. Unexpected error."));
            }
        }

        #endregion

        #region Message Sending Helpers
        /// <summary>
        /// Sends a message to the client and closes the connection.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="message"> The message to be sent. </param>
        private async Task SendAndCloseAsync(TcpClient client, InfoMessage message)
        {
            await SafeSendAsync(client, message);
            client.Close();
        }

        /// <summary>
        /// Safely sends a message to the client, logging any exceptions.
        /// </summary>
        /// <param name="client"> The connected TCP client. </param>
        /// <param name="message"> The message to be sent. </param>
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

        #region Logging

        /// <summary>
        /// OnLog event handler to route log messages to the logger service.
        /// </summary>  
        /// <param name="sender"> The sender of the log event. </param>
        /// <param name="e"> The log event arguments. </param>
        private void OnLog_Delegate(object? sender, OnLogEventArgs e)
        {
            loggerService.Log(e.Message, e.LogLevel, sender);
        }

        #endregion

    }
}
