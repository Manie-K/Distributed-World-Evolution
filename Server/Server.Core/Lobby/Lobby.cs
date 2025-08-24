using System.Net.Sockets;
using Server.Core.Logging;
using SharedLibrary;
using Server.Shared;

namespace Server.Core.Lobby
{
    internal class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
        /// <inheritdoc/>
        public int LobbyId { get; private init; }


        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;  

        private List<TcpClient> clients;
        private bool running;

        private IEnumerable<WorldEntity> entities;

        public static event EventHandler<OnLogEventArgs>? OnLog;

        /// <summary>
        /// Default constructor for Lobby.
        /// <paramref name="id"/> Unique identifier for the lobby.
        /// </summary>
        public Lobby(int id)
        {
            LobbyId = id;

            entities = new List<WorldEntity>();
            clients = new List<TcpClient>();
            running = true;
            Server.OnMessageFromClientReceived += OnMessageFromClientReceived_Delegate;
        }

        public void Run()
        {
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            while (running)
            {
                Task.Delay((int)((1 / LOBBY_UPDATES_PER_SECOND) * 1000));
                PublishWorldState();
            }

            Log($"Lobby {LobbyId} closed.", LogLevelEnum.Info);
        }

        private void PublishWorldState()
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    MessageManager.SendMessage(client, new WorldStateMessage(
                            entities.Select(e => e.ToDTO())
                        ));
                }
            }
        }
        private void UpdateState(MessageBase message, TcpClient client)
        {
            if (message == null)
            {
                Log("Received null message from client.", LogLevelEnum.Warning);
                return;
            }
            else
            {
                switch (message.MessageType)
                {
                    case MessageTypeEnum.EntityState:
                        HandleUpdateWorldEntityStateMessage(client, (EntityStateMessage)message);
                        break;
                    case MessageTypeEnum.UserState:
                        HandleUpdateUserStateMessage(client, (UserStateMessage)message);
                        break;
                    case MessageTypeEnum.InfoMessage:
                        HandleInfoMessage(client, (InfoMessage)message);
                        break;
                    default:
                        HandleUnsupportedMessageType(client, message);
                        break;
                }
            }
                    
        }

        #region Delegates

        private void OnMessageFromClientReceived_Delegate(OnMessageFromClientEventArgs args)
        {
            lock(clients)
            {
                if(!clients.Contains(args.Client))
                {
                    return;
                }
            }

            UpdateState(args.Message, args.Client);
        }

        #endregion

        #region Handlers

        private void HandleUpdateWorldEntityStateMessage(TcpClient client, EntityStateMessage message)
        {
            WorldEntityDTO ent = message.Entity;
            if (ent == null)
            {
                throw new Exception("Received null WorldEntityDTO in EntityStateMessage.");
            }

            WorldEntity? existingEntity = entities.Where(e => e.Id == ent.Id).First();

            if (existingEntity == null)
            {
                Log($"Entity with ID {ent.Id} not found in lobby {LobbyId}.", LogLevelEnum.Error);
                return;
            }
            
            existingEntity.UpdateStateWithDTO(ent.State);
        }

        private void HandleUpdateUserStateMessage(TcpClient client, UserStateMessage message)
        {
            throw new NotImplementedException("UserStateMessage handling is not implemented yet.");
        }

        private void HandleInfoMessage(TcpClient client, InfoMessage message)
        {
            throw new NotImplementedException("InfoMessage handling is not implemented yet.");
        }

        private void HandleUnsupportedMessageType(TcpClient client, MessageBase message)
        {
            Log($"Received unsupported message type: {message.MessageType}", LogLevelEnum.Error);
            throw new NotImplementedException($"Unsupported message type: {message.MessageType}.");
        }

        #endregion

        #region Helpers
        public void AddClient(TcpClient client)
        {
            lock (clients)
            {
                clients.Add(client);
            }

            MessageManager.SendMessage(client, new InfoMessage($"You have joined lobby {LobbyId}.\n"));
        }

        #endregion

        #region Logging
        private void Log(Exception ex, LogLevelEnum level)
        {
            Log(ex.Message, level);
        }

        private void Log(string message, LogLevelEnum level)
        {
            OnLogEventArgs args = new OnLogEventArgs
            (message, level);

            OnLog?.Invoke(this, args);
        }
        #endregion
    }
}
