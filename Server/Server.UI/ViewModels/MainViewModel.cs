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
            var lobbiesFromServer = new List<string> { "one", "two", "three" };

            foreach (var lobby in lobbiesFromServer)
            {
                Tabs.Add(new LobbyViewModel("name", "info"));
            }
        }

        private void SubscribeToLogs()
        {
            //TODO: change
            string serverIp = "127.0.0.1";
            int port = 5000;

            TcpClient client = new TcpClient(serverIp, port);
            Thread.Sleep(3000);
            MessageManager.SendMessage(client, new RoleMessage(RoleEnum.UI));

            Thread receiveThread = new Thread(() =>
            {
                MessageBase message = MessageManager.ReceiveMessage(client);
                LogMessage log = (LogMessage)message;
                HandleLog(log.Sender, log.OnLogEventArgs);
            });
            receiveThread.Start();
        }

        public void HandleLog(object? sender, OnLogEventArgs e)
        {
            if (sender is Server.Core.Server)
            {
                ServerTab.AppendLog(e.Message, e.Timestamp, e.LogLevel);
            }
            else if (sender is Server.Core.Lobby.Lobby lobby)
            {
                
            }
        }

    }

}
