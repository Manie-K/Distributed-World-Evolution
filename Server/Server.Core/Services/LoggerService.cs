using Microsoft.Extensions.Hosting;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net.Sockets;

namespace Server.Core.Services
{
    public class LoggerService : BackgroundService
    {
        private readonly ConcurrentQueue<MessageBase> _messageQueue = new();
        private readonly TimeSpan _pollInterval = TimeSpan.FromMilliseconds(50);
        private List<TcpClient> ClientsUI { set; get; } = new List<TcpClient>();

        private readonly object _clientsLock = new object();

        public void AddClient(TcpClient clientUI)
        {
            ClientsUI.Add(clientUI);
        }

        public void SendLobbyDTO(LobbyDTO lobbyDto)
        {
            lock (_clientsLock)
            {
                foreach (var ClientUI in ClientsUI)
                {
                    _messageQueue.Enqueue(new LobbyDataMessage(lobbyDto, Guid.Empty));
                }
            }
        }

        public void Log(string message, LogLevelEnum logLevel, object? sender = null)
        {
            var args = new OnLogEventArgs(message, logLevel);
            int senderID = (sender is Lobby.Lobby lobby) ? lobby.LobbyId : -1;
            var logMessage = new LogMessage(args, senderID);

            var color = args.LogLevel switch
            {
                LogLevelEnum.Debug => Color.White,
                LogLevelEnum.Info => Color.Green,
                LogLevelEnum.Warning => Color.Yellow,
                LogLevelEnum.Error => Color.OrangeRed,
                LogLevelEnum.Critical => Color.Red,
                _ => Color.Gray,
            };

            Console.WriteLine($"[{args.Timestamp:HH:mm:ss}] [{args.LogLevel}] {args.Message}");

            _messageQueue.Enqueue(logMessage);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[LoggerService] Logger service started working...");

            while (!stoppingToken.IsCancellationRequested)
            {
                while (_messageQueue.TryDequeue(out var message))
                {
                    lock (_clientsLock)
                    {
                        foreach (var ClientUI in ClientsUI)
                        {
                            if (ClientUI.Connected)
                            {
                                try
                                {
                                    _ = MessageManager.SendMessageAsync(ClientUI, message);
                                }
                                catch (IOException)
                                {
                                    ClientsUI.Remove(ClientUI);
                                    ClientUI.Close();
                                }
                            }
                            else
                            {
                                ClientsUI.Remove(ClientUI);
                                ClientUI.Close();
                            }
                        }
                    }
                }

                await Task.Delay(_pollInterval, stoppingToken);
            }

            Console.WriteLine("[LoggerService] Logger service stopped working.");
        }

    }
}
