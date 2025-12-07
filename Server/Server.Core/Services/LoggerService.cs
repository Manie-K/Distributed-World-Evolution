using Microsoft.Extensions.Hosting;
using SharedLibrary.DTOs.LobbyDTO;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using System.Collections.Concurrent;
using System.Drawing;
using System.Net.Sockets;

namespace Server.Core.Services
{
    /// <summary>
    /// Background service for logging messages and sending them to connected UI clients.
    /// </summary>
    public class LoggerService : BackgroundService
    {
        /// <summary>
        /// Queue to hold log messages to be sent to UI clients.
        /// </summary>
        private readonly ConcurrentQueue<MessageBase> _messageQueue = new();

        /// <summary>
        /// Polling interval for checking the message queue.
        /// </summary>
        private readonly TimeSpan _pollInterval = TimeSpan.FromMilliseconds(50);

        /// <summary>
        /// Lock object for thread-safe access to the clients list.
        /// </summary>
        private readonly object _clientsLock = new object();

        /// <summary>
        /// UI Clients connected to the logger service.
        /// </summary>
        private List<TcpClient> ClientsUI { set; get; } = new List<TcpClient>();


        /// <summary>
        /// Adds a new UI client to the logger service.
        /// </summary>
        /// <param name="clientUI"> The TCP client representing the UI. </param>
        public void AddClient(TcpClient clientUI)
        {
            ClientsUI.Add(clientUI);
        }

        /// <summary>
        /// Sends the provided LobbyDTO to all connected UI clients.
        /// </summary>
        /// <param name="lobbyDto"> The LobbyDTO to send. </param>
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

        /// <summary>
        /// Logs a message with the specified content and log level.
        /// </summary>
        /// <param name="content"> The content of the log message. </param>
        /// <param name="logLevel"> The log level of the message. </param>
        /// <param name="sender"> Optional sender object, used to identify the source of the log. </param>
        public void Log(string content, LogLevelEnum logLevel, object? sender = null)
        {
            Log log = new Log(content, logLevel);
            int senderID = (sender is Lobby.Lobby lobby) ? lobby.LobbyId : -1;
            var logMessage = new LogMessage(log, senderID);

            var color = logLevel switch
            {
                LogLevelEnum.Debug => Color.White,
                LogLevelEnum.Info => Color.Green,
                LogLevelEnum.Warning => Color.Yellow,
                LogLevelEnum.Error => Color.OrangeRed,
                LogLevelEnum.Critical => Color.Red,
                _ => Color.Gray,
            };

            Console.WriteLine($"[{log.Timestamp:HH:mm:ss}] [{log.LogLevel}] {log.Content}");

            _messageQueue.Enqueue(logMessage);
        }

        /// <inheritdoc/>
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