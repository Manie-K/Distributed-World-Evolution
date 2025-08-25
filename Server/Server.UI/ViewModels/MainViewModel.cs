using Server.Core;
using Server.UI.Models;
using SharedLibrary;
using SharedLibrary.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.UI.ViewModels
{
    internal class MainViewModel
    {
        public ServerViewModel ServerTab { get; }
        public ObservableCollection<BaseTabViewModel> Tabs { get; } = new();

        public MainViewModel()
        {
            Tabs.Clear();

            ServerTab = new ServerViewModel();
            Tabs.Add(ServerTab);

            LoadFromServer();

            SubscribeToLogs();
        }

        private void LoadFromServer()
        {
            var lobbiesFromServer = new List<int> { 1, 2, 3 };

            foreach (var lobby in lobbiesFromServer)
            {
                Tabs.Add(new LobbyViewModel("name", "info", lobby));
            }
        }

        private void SubscribeToLogs()
        {
            //TODO: change
            string serverIp = "127.0.0.1";
            int port = 5000;

            TcpClient client = new TcpClient(serverIp, port);
            _ = MessageManager.SendMessageAsync(client, new RoleMessage(RoleEnum.UI));

            Thread receiveThread = new Thread(() =>
            {
                while (true)
                {
                    MessageBase message = MessageManager.ReceiveMessage(client);

                    if (message.MessageType == MessageTypeEnum.LogMessage)
                    {
                        LogMessage log = (LogMessage)message;
                        HandleLog(log.SenderID, log.OnLogEventArgs);
                    }
                }

            });
            receiveThread.Start();
        }

        public void HandleLog(int senderID, OnLogEventArgs e)
        {
            if (senderID != 0)
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
            return Tabs.OfType<LobbyViewModel>().FirstOrDefault(tab => tab.LobbyID == lobbyId);
        }
    }

}
