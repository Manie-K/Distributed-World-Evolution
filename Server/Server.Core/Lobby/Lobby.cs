using System.Net.Sockets;
using SharedLibrary;
using Server.Shared;
using Server.Core.Modules;

namespace Server.Core.Lobby
{
    public class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
        /// <inheritdoc/>
        public int LobbyId { get; private init; }

        public static event EventHandler<OnLogEventArgs>? OnLog;
        
        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;  

        private List<TcpClient> clients;
        private ICollection<WorldEntity> entities;
        private ICollection<Module> loadedModules;
        private Dictionary<WorldEntity, FrameEntityMetadata> metadata;

        private bool running;


        /// <summary>
        /// Default constructor for Lobby.
        /// <paramref name="id"/> Unique identifier for the lobby.
        /// </summary>
        public Lobby(int id)
        {
            LobbyId = id;

            entities = new List<WorldEntity>(); //Currently no way to add them.
            loadedModules = new List<Module>(); //Currently no way to add them.
            metadata = new Dictionary<WorldEntity, FrameEntityMetadata>();
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
            foreach (var entity in entities)
            {
                if (metadata.ContainsKey(entity))
                {
                    metadata[entity].AlreadyChangedPosition = false;
                }
            }

            lock (clients)
            {
                foreach (var client in clients)
                {
                    _= MessageManager.SendMessageAsync(client, new WorldStateMessage(
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

        private void SimulateEntityUpdate(WorldEntity entity, EntityStateDTO newState)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }
        
            if(entity.State.Position != newState.Position && !metadata[entity].AlreadyChangedPosition)
            {
                if (!entities.Where(e => e.State.Position == entity.State.Position).Any())
                {
                    entity.State.Position = newState.Position;
                    metadata[entity].AlreadyChangedPosition = true;
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
            
            SimulateEntityUpdate(existingEntity, ent.State);
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

        public bool AddClient(TcpClient client)
        {
            lock (clients)
            {
                if (clients.Contains(client))
                {
                    Log("Client already in lobby.", LogLevelEnum.Warning);
                    return false;
                }
                clients.Add(client);
            }

            _ = MessageManager.SendMessageAsync(client, new InfoMessage($"You have joined lobby {LobbyId}.\n"));
            return true;
        }

        public bool LoadModule(Module module)
        {
            lock (loadedModules)
            {
                if (loadedModules.Contains(module))
                {
                    Log($"Module {module.Name} already loaded in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                loadedModules.Add(module);
                return true;
            }
        }

        // What do we expect here? Just remove in future or present?
        public bool UnloadModule(Module module)
        {
            lock (loadedModules)
            {
                if (!loadedModules.Contains(module))
                {
                    Log($"Module {module.Name} isn't loaded in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                loadedModules.Remove(module);
                return true;
            }
        }

        // We should decide how we will handle creating and destroying world entities
        public bool AddWorldEntity(WorldEntity entity)
        {
            lock (entities)
            {
                if (entities.Contains(entity))
                {
                    Log($"Entity {entity.Id} already exists in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                if(!loadedModules.Contains(entity.Module))
                {
                    Log($"Entity's {entity.Id} module {entity.Module.Name} is not loaded in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                entities.Add(entity);
                metadata.Add(entity, new FrameEntityMetadata());
                return true;
            }
        }

        public bool DestroyWorldEntity(WorldEntity entity)
        {
            lock (entities)
            {
                if (!entities.Contains(entity))
                {
                    Log($"Entity {entity.Id} does not exist in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                entities.Remove(entity);
                metadata.Remove(entity);
                return true;
            }
        }

        
        public bool SaveWorldState()
        {
            throw new NotImplementedException("Saving is not implemented yet.");
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
