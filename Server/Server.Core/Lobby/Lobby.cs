using System.Net.Sockets;
using SharedLibrary;
using Server.Core;
using Server.Core.Modules;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Server.Core.Lobby
{
    public class Lobby : ILobby
    {
        //TODO: add modules when they are implemented
        /// <inheritdoc/>
        public int LobbyId { get; private init; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }


        public static event EventHandler<OnLogEventArgs>? OnLog;
        
        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;  

        private List<TcpClient> clients;
        private ICollection<WorldEntity> entities;
        private ICollection<Module> loadedModules;
        private Dictionary<WorldEntity, FrameEntityMetadata> metadata;
        private bool[,] walkableTile;

        private bool running;


        public static Lobby CreateLobby(int id, string name, int maxPlayers, int mapId, IEnumerable<int> moduleIDs, IModuleService moduleService)
        {
            Lobby lobby = new Lobby(id, name, maxPlayers, mapId);
            
            foreach(int mId in moduleIDs)
            {
                Module module = moduleService.GetModuleById(mId);
                if (module == null)
                {
                    throw new Exception($"Module with ID {mId} not found.");
                }

                lobby.LoadModule(module);
            }
            return lobby;
        }

        private Lobby(int id, string name, int maxPlayers, int mapId)
        {
            LobbyId = id;
            Name = name;
            MaxPlayers = maxPlayers;
            walkableTile = null; //TODO: Implement map

            entities = new List<WorldEntity>(); //Currently no way to add them.
            loadedModules = new List<Module>();
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
            //TODO: @FranciszekGwarek Here we need to simulate non-human entities?

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

        private void UpdateServerState(MessageBase message, TcpClient client)
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
        
            // If we change position, there is a possible new interaction 
            if(entity.State.Position != newState.Position && !metadata[entity].AlreadyChangedPosition)
            {
                Module module = loadedModules.Where(m => m.ID == entity.ModuleID).First();
                
                Type interactionType = GetInteractionType(entity, newState);
                IBehaviour behaviour = module.GetBehaviourOfType(interactionType);

                if (interactionType == typeof(IMoveBehaviour)) 
                {
                    ((IMoveBehaviour)behaviour).Move();
                }
                else if(interactionType == typeof(IAttackBehaviour)) 
                {
                    WorldEntity target = entities.Where(e => e.State.Position == newState.Position).First();
                    ((IAttackBehaviour)behaviour).Attack(entity, target);
                }
                // ...
            }

        }

        private Type GetInteractionType(WorldEntity entity, EntityStateDTO newState)
        {
            WorldEntity? entityOnPosition = entities.Where(ent => ent.State.Position == newState.Position).FirstOrDefault();

            if (entityOnPosition == null)
            {
                return typeof(IMoveBehaviour);
            }

            EntityTypeEnum entityType = entity.Type;
            EntityTypeEnum targetType = entityOnPosition.Type;

            //Here we need to establish possible interactions
            if ((entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Animal) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
                )
            {
                Module entityModule = ModuleService.Instance.GetModuleById(entity.ModuleID);
                IAttackBehaviour attackBehaviour = (IAttackBehaviour)entityModule.GetBehaviourOfType(typeof(IAttackBehaviour));
                if (attackBehaviour.ShouldAttack(entity, entityOnPosition))
                {
                    return typeof(IAttackBehaviour);
                }
            }

            return null;
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

            UpdateServerState(args.Message, args.Client);
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

        public bool RemoveClient(TcpClient client)
        {
            lock (clients)
            {
                if (!clients.Contains(client))
                {
                    Log("Client already not in lobby.", LogLevelEnum.Warning);
                    return false;
                }
                clients.Remove(client);
            }

            _ = MessageManager.SendMessageAsync(client, new InfoMessage($"You have disjoined lobby {LobbyId}.\n"));
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

                bool moduleLoaded = loadedModules.Any(m => m.ID == entity.ModuleID);
                if (!moduleLoaded)
                {
                    Log($"Entity's {entity.Id} module is not loaded in lobby {LobbyId}.", LogLevelEnum.Warning);
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
