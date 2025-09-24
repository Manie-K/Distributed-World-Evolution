using Server.Core;
using Server.UI.Models;
using SharedLibrary;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using SharedLibrary.DTOs.LobbyDTO;

namespace Server.UI.ViewModels
{
    internal class MainViewModel
    {
        public ServerViewModel ServerTab { get; }
        public ObservableCollection<BaseTabViewModel> Tabs { get; } = new();

        private readonly LobbyService _service = new LobbyService();
        private TcpClient _client;

        public MainViewModel()
        {
            //TODO: change to config
            string serverIp = "127.0.0.1";
            int port = 5000;

            _client = new TcpClient(serverIp, port);

            Tabs.Clear();

            ServerTab = new ServerViewModel();
            Tabs.Add(ServerTab);

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            //TODO: change!!!
            await MessageManager.SendMessageAsync(_client, new RoleMessage(RoleEnum.UI));
            await MessageManager.SendMessageAsync(_client, new GetMessage(GetMessageTypeEnum.GetAllLobbies));

            MessageBase message = await MessageManager.ReceiveMessageAsync(_client);
            List<LobbyDTO> lobbies = (List<LobbyDTO>)((LobbyListMessage)message).Lobbies;

            foreach (LobbyDTO lobby in lobbies)
            {
                Tabs.Add(new LobbyViewModel(lobby.ID, lobby.Name, lobby.MaxPlayers));
            }

            _ = Task.Run(async () =>
            {
                while (true)
                {
                    MessageBase message = await MessageManager.ReceiveMessageAsync(_client);

                    if (message.MessageType == MessageTypeEnum.LogMessage)
                    {
                        LogMessage log = (LogMessage)message;
                        HandleLog(log.SenderID, log.OnLogEventArgs);
                    }
                }
            });

        }

        public void HandleLog(int senderID, OnLogEventArgs e)
        {
            if (senderID != -1)
            {
                LobbyViewModel? lobby = FindLobbyTabById(senderID);
                if (lobby != null)
                {
                    lobby.AppendLog(e);
                }
                else
                {
                    ServerTab.AppendLog(e);
                }
            }
            else
            {
                ServerTab.AppendLog(e);
            }
        }

        private LobbyViewModel? FindLobbyTabById(int lobbyId)
        {
            return Tabs.OfType<LobbyViewModel>().FirstOrDefault(tab => tab.ID == lobbyId);
        }
    }

}
