using System.Net.Sockets;
using Server.Core.Logging;
using SharedLibrary;

namespace Server.Core.Lobby
{
    internal class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
        /// <inheritdoc/>
        public int LobbyId { get; private init; }

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
        }


        public void AddClient(TcpClient client)
        {
            lock (clients)
            {
                clients.Add(client);
            }

            MessageManager.SendMessage(client, new DefaultMessage($"You have joined lobby {LobbyId}.\n"));
        }

        public void Run()
        {
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            while (running)
            {
                Update();
                Thread.Sleep(10);
            }

            Log($"Lobby {LobbyId} closed.", LogLevelEnum.Info);
        }

        private void Update()
        {
            lock (clients)
            {
                foreach (var client in clients)
                {
                    if (client.Available > 0)
                    {
                        //Chat communication between users
                        MessageBase message = GetMessageFromClient(client);

                        if (message == null)
                        {
                            Log("Received null message from client.", LogLevelEnum.Warning);
                            continue;
                        }
                        else
                        {
                            switch (message.MessageType)
                            {
                                case MessageTypeEnum.CreateLobby:
                                    HandleCreateLobbyMessage(client, (CreateLobbyMessage)message);
                                    break;
                                case MessageTypeEnum.UpdateWorldEntityState:
                                    HandleUpdateWorldEntityStateMessage(client, (UpdateWorldEntityStateMessage)message);
                                    break;
                                case MessageTypeEnum.RefreshWorldEntities:
                                    HandleRefreshWorldEntitiesMessage(client, (RefreshWorldEntitiesMessage)message);
                                    break;
                                case MessageTypeEnum.UpdateUserState:
                                    HandleUpdateUserStateMessage(client, (UpdateUserStateMessage)message);
                                    break;
                                case MessageTypeEnum.JoinLobby:
                                    HandleJoinLobbyMessage(client, (JoinLobbyMessage)message);
                                    break;
                                case MessageTypeEnum.DefaultMessage:
                                    HandleDefaultMessage(client, (DefaultMessage)message);
                                    break;
                                default:
                                    HandleUnsupportedMessageType(client, message);
                                    continue;
                            }
                        }
                    }
                }
            }
        }


        #region Handlers

        private void HandleCreateLobbyMessage(TcpClient client, CreateLobbyMessage message)
        {
            throw new NotImplementedException("CreateLobbyMessage handling is not implemented yet.");
        }

        private void HandleUpdateWorldEntityStateMessage(TcpClient client, UpdateWorldEntityStateMessage message)
        {
            WorldEntity ent = message.Entity;
            if (ent == null)
            {
                throw new Exception("Received null WorldEntity in UpdateWorldEntityStateMessage.");
            }

            WorldEntity? existingEntity = entities.Where(e => e.Id == ent.Id).First();

            if (existingEntity == null)
            {
                Log($"Entity with ID {ent.Id} not found in lobby {LobbyId}.", LogLevelEnum.Error);
                return;
            }
            else
            {
                existingEntity.UpdateState(ent.State);
            }

            RefreshWorldEntitiesMessage refreshMessage = new RefreshWorldEntitiesMessage(entities);
            foreach (var c in clients)
            {
                if(c == client) continue; // Do not send the message back to the sender
                SendMessageToClient(c, refreshMessage);
            }
        }

        private void HandleRefreshWorldEntitiesMessage(TcpClient client, RefreshWorldEntitiesMessage message)
        {
            throw new NotImplementedException("RefreshWorldEntitiesMessage handling is not implemented yet.");
        }

        private void HandleUpdateUserStateMessage(TcpClient client, UpdateUserStateMessage message)
        {
            throw new NotImplementedException("UpdateUserStateMessage handling is not implemented yet.");
        }

        private void HandleJoinLobbyMessage(TcpClient client, JoinLobbyMessage message)
        {
            throw new NotImplementedException("JoinLobbyMessage handling is not implemented yet.");
        }

        private void HandleDefaultMessage(TcpClient client, DefaultMessage message)
        {
            throw new NotImplementedException("DefaultMessage handling is not implemented yet.");
        }

        private void HandleUnsupportedMessageType(TcpClient client, MessageBase message)
        {
            Log($"Received unsupported message type: {message.MessageType}", LogLevelEnum.Error);
            throw new NotImplementedException($"Unsupported message type: {message.MessageType}.");
        }

        #endregion

        #region Helpers
        private MessageBase GetMessageFromClient(TcpClient client)
        {
            MessageBase message = MessageManager.ReceiveMessage(client);

            return message;
        }

        private void SendMessageToClient(TcpClient client, MessageBase message)
        {
            MessageManager.SendMessage(client, message);
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
