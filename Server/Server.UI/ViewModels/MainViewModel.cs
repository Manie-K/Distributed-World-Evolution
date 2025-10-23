using Server.UI.Models;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using SharedLibrary.DTOs.LobbyDTO;

namespace Server.UI.ViewModels
{
    internal class MainViewModel
    {
        public ServerViewModel ServerTab { get; }
        public ObservableCollection<BaseTabViewModel> Tabs { get; } = new();

        private TcpClient _client;

        public MainViewModel()
        {
            //TODO: change to config
            string serverIp = "127.0.0.1";
            int port = 8080;

            _client = new TcpClient(serverIp, port);

            Tabs.Clear();

            ServerTab = new ServerViewModel();
            Tabs.Add(ServerTab);

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await MessageManager.SendMessageAsync(_client, new RoleMessage(RoleEnum.UI));
            await MessageManager.SendMessageAsync(_client, new GetMessage(GetMessageTypeEnum.LobbyList));

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
                    else if (message.MessageType == MessageTypeEnum.LobbyList)
                    {
                        List<LobbyDTO> lobbies = (List<LobbyDTO>)((LobbyListMessage)message).Lobbies;

                        foreach (LobbyDTO lobby in lobbies)
                        {
                            _ = App.Current.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                Tabs.Add(new LobbyViewModel(lobby.ID, lobby.Name, lobby.MaxPlayers, lobby.CurrentPlayers, lobby.MapID));
                            }));
                        }

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
