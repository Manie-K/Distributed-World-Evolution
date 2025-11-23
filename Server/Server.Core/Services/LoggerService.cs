using Microsoft.Extensions.Hosting;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net.Sockets;

namespace Server.Core.Services
{
    public class LoggerService : BackgroundService
    {
        private readonly ConcurrentQueue<LogMessage> _logQueue = new();
        private readonly TimeSpan _pollInterval = TimeSpan.FromMilliseconds(50);
        public TcpClient? ClientUI { set; get; }

        public void SendLobby(Lobby.ILobby lobby)
        {
            if (ClientUI != null && ClientUI.Connected)
            {
                _ = MessageManager.SendMessageAsync(ClientUI, new LobbyDataMessage(lobby.ToDTO(), null));
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

            _logQueue.Enqueue(logMessage);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("[LoggerService] Logger service working...");

            while (!stoppingToken.IsCancellationRequested)
            {
                if (ClientUI != null && ClientUI.Connected)
                {
                    if(_logQueue.TryDequeue(out var logMessage))
                    {
                        await MessageManager.SendMessageAsync(ClientUI, logMessage);
                    }
                }
                else
                {
                    await Task.Delay(_pollInterval, stoppingToken);
                }
            }

            Console.WriteLine("[LoggerService] Logger service stopped working.");
        }

    }
}
