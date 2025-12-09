using Microsoft.Extensions.Configuration;
using Server.UI.Models;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace Server.UI.ViewModels
{
    /// <summary>
    /// Class representing the main view model for the server UI.
    /// </summary>
    internal class MainViewModel
    {
        /// <summary>
        /// Server tab view model.
        /// </summary>
        public ServerViewModel ServerTab { get; }

        /// <summary>
        /// Observable collection of tab view models.
        /// </summary>
        public ObservableCollection<BaseTabViewModel> Tabs { get; } = new();

        /// Client for TCP communication with the server.
        private TcpClient _client;

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainViewModel()
        {
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            string serverIp = config["TcpServerConnection:Host"]!;
            int port = int.Parse(config["TcpServerConnection:Port"]!);

            _client = new TcpClient(serverIp, port);

            Tabs.Clear();

            ServerTab = new ServerViewModel(-1);
            Tabs.Add(ServerTab);

            InitializeAsync();
        }

        /// Initializes the view model asynchronously.
        private async void InitializeAsync()
        {
            await MessageManager.SendMessageAsync(_client, new RoleMessage(RoleEnum.UI));
            
            MessageBase message = await MessageManager.ReceiveMessageAsync(_client);
            List<LobbyDTO> lobbies = (List<LobbyDTO>)((LobbyListMessage)message).Lobbies;

            foreach (LobbyDTO lobby in lobbies)
            {
                Tabs.Add(new LobbyViewModel(lobby.ID, lobby.Name, lobby.MaxPlayers, lobby.CurrentPlayers, lobby.MapID));
            }

            while (true)
            {
                message = await MessageManager.ReceiveMessageAsync(_client);

                if (message.MessageType == MessageTypeEnum.LogMessage)
                {
                    LogMessage logMessage = (LogMessage)message;
                    HandleLog(logMessage.SenderID, logMessage.Log);
                }
                else if (message.MessageType == MessageTypeEnum.LobbyData)
                {
                    LobbyDataMessage lobbyData = (LobbyDataMessage)message;
                    LobbyDTO lobbyDto = lobbyData.Lobby;
                    HandleLobby(lobbyDto);
                }

            }

        }

        /// <summary>
        /// Handles a log message.
        /// </summary>
        /// <param name="senderID"> ID of the sender. </param>
        /// <param name="log"> Log message. </param>
        private void HandleLog(int senderID, Log log)
        {
            if (senderID != -1)
            {
                LobbyViewModel? lobby = GetLobbyTabById(senderID);
                if (lobby != null)
                {
                    lobby.AppendLog(log);
                }
                else
                {
                    ServerTab.AppendLog(log);
                }
            }
            else
            {
                ServerTab.AppendLog(log);
            }
        }

        /// <summary>
        /// Handles lobby data.
        /// </summary>
        /// <param name="lobbyDto"> Lobby data transfer object. </param>
        private void HandleLobby(LobbyDTO lobbyDto)
        {
            var existing = Tabs.FirstOrDefault(t => t.ID == lobbyDto.ID);
            if (existing is LobbyViewModel vm)
            {
                vm.Info = $"ID: {lobbyDto.ID}, Max players: {lobbyDto.MaxPlayers}, Current Players: {lobbyDto.CurrentPlayers}, MapID: {lobbyDto.MapID}";
            }
            else
            {
                Tabs.Add(new LobbyViewModel(
                    lobbyDto.ID,
                    lobbyDto.Name,
                    lobbyDto.MaxPlayers,
                    lobbyDto.CurrentPlayers,
                    lobbyDto.MapID
                ));
            }
        }

        /// <summary>
        /// Retrieves a lobby tab by its ID.
        /// </summary>
        /// <param name="lobbyId"> ID of the lobby. </param>
        private LobbyViewModel? GetLobbyTabById(int lobbyId)
        {
            return Tabs.OfType<LobbyViewModel>().FirstOrDefault(tab => tab.ID == lobbyId);
        }

    }

}