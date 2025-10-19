using System.Net.Sockets;
using SharedLibrary;
using Server.Core;
using Server.Core.Modules;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using SharedLibrary.DTOs.EntitiesDTO;
using Server.Core.Behaviours;
using Server.Core.Helpers;
using Server.Core.Services;

namespace Server.Core.Lobby
{
    public class Lobby : ILobby
    {
        /// <inheritdoc/>
        public int LobbyId { get; private init; }
        public string Name { get; set; }
        public int MaxPlayers { get; set; }

        public static event EventHandler<OnLogEventArgs>? OnLog;
        
        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;

        private readonly IModuleService moduleService;
        private readonly List<TcpClient> clients;
        private readonly ICollection<WorldEntity> entities;
        private readonly ICollection<int> allowedModulesIDs;
        private bool[,] walkableTiles;

        private bool running;

        public static Lobby CreateLobby(int id, string name, int maxPlayers, int mapId, IEnumerable<int> moduleIDs, IModuleService moduleService)
        {
            Lobby lobby = new Lobby(id, name, maxPlayers, mapId, moduleService);
            
            foreach(int mId in moduleIDs)
            {
                Module? module = moduleService.GetModuleById(mId);
                if (module == null)
                {
                    throw new Exception($"ModuleDTO with ID {mId} not found.");
                }

                lobby.AddAllowedModule(mId);
            }

            lobby.InitializeWorldEntities();

            return lobby;
        }

        private Lobby(int id, string name, int maxPlayers, int mapId, IModuleService moduleService)
        {
            LobbyId = id;
            Name = name;
            MaxPlayers = maxPlayers;
            walkableTiles = null; //TODO: Implement map
            this.moduleService = moduleService;

            entities = new List<WorldEntity>(); //Currently no way to add them.
            allowedModulesIDs = new List<int>();
            clients = new List<TcpClient>();
            running = true;

            Server.OnMessageFromClientReceived += OnMessageFromClientReceived_Delegate;
        }

        public void Run()
        {
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            while (running)
            {
                Task.Delay((int)((1 / LOBBY_UPDATES_PER_SECOND) * 1000)).Wait();
                PublishWorldState();
            }

            Log($"Lobby {LobbyId} closed.", LogLevelEnum.Info);
        }

        private void InitializeWorldEntities()
        {
            Log("Initializing world entities...", LogLevelEnum.Info);
            throw new NotImplementedException();
        }

        private void PublishWorldState()
        {
            // Simulate all non-human entities
            Module? entModule;
            EntityState nextState;

            foreach (var entity in entities)
            {
                if(entity.State.InteractionFramesLeft > 0)
                {
                    continue;
                }

                entModule = moduleService.GetModuleById(entity.ModuleID);
                if(entModule == null)
                {
                    Log($"Entity's {entity.Id} module not found in lobby {LobbyId}.", LogLevelEnum.Warning);
                    continue;
                }

                if (entModule.Type == EntityTypeEnum.Human)
                {
                    continue;
                }

                var moveBehaviour = entModule.GetBehaviourOfType(typeof(MoveBehaviourBase));
                nextState = new EntityState(entity.State);

                (int stepX, int stepY) = ((MoveBehaviourBase)moveBehaviour).GetNextMovement(entity);
                nextState.Position.X += stepX;
                nextState.Position.Y += stepY;

                SimulateNonHumanEntityUpdate(entity, nextState.ToDTO());
            }

            lock (clients)
            {
                foreach (var client in clients)
                {
                    _ = MessageManager.SendMessageAsync(client, new WorldStateMessage(
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
                    case MessageTypeEnum.UserInteraction:
                        HandleUpdateWorldEntityStateMessage(client, (UserInteractionMessage)message);
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

        private void SimulateNonHumanEntityUpdate(WorldEntity entity, EntityStateDTO newState)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            Module? entModule = moduleService.GetModuleById(entity.ModuleID)
                ?? throw new Exception($"Entity's {entity.Id} module not found.");

            // If we change position, there is a possible new interaction 
            if (entity.State.Position != newState.Position && entity.State.InteractionFramesLeft == 0)
            {
                Type? interactionType = GetInteractionType(entity, newState);
                if (interactionType is null) return;

                WorldEntity targetEntity = entities.Where(e => e.State.Position == newState.Position).First();
                IBehaviour behaviour = entModule.GetBehaviourOfType(interactionType);


                // In case when we need to add custom parameters
                if(interactionType == typeof(MoveBehaviourBase))
                {
                    behaviour.Execute(entity, null, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_PARAM, walkableTiles   },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position }
                    });
                }
                else
                {
                    behaviour.Execute(entity, targetEntity);
                    entity.State.InteractionFramesLeft = 10;
                    targetEntity.State.InteractionFramesLeft = 10;
                }

            }

        }
        private void SimulateHumanEntityUpdate(WorldEntityDTO human, WorldEntityDTO? other)
        {
            WorldEntity entHuman = entities.Where(e => e.Id == human.Id).First();
            WorldEntity? entOther = entities.Where(e => e.Id == other?.Id).FirstOrDefault();

            if (entHuman == null)
            {
                throw new ArgumentNullException(nameof(entHuman), "Entity not found");
            }

            /// @FranciszekGwarek - sanity check please
            if(human.State.InteractionFramesLeft > 0)
            {
                return;
            }

            entHuman.UpdateState(new EntityState(human.State));
            entOther?.UpdateState(new EntityState(other!.State)); //If entityOther isn't null, then it's dto also isn't.
        }


        private Type? GetInteractionType(WorldEntity entity, EntityStateDTO newState)
        {
            WorldEntity? entityOnPosition = entities.Where(ent => ent.State.Position == newState.Position).FirstOrDefault();

            if (entityOnPosition == null)
            {
                return typeof(MoveBehaviourBase);
            }

            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            EntityTypeEnum entityType = entityModule.Type;
            EntityTypeEnum targetType = moduleService.GetModuleById(entityOnPosition.ModuleID)?.Type ?? throw new Exception($"Module with ID={entity.ModuleID} not found"); ;

            // We refactored this so that humans dont use this method, the send the new states in frames
            if (entityType == EntityTypeEnum.Human) return null;

            // Attack - old code, Humans wont be here
            if ((entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Animal) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
                )
            {
                AttackBehaviourBase attackBehaviour = (AttackBehaviourBase)entityModule.GetBehaviourOfType(typeof(AttackBehaviourBase));
                if (attackBehaviour.CanExecute(entity, entityOnPosition))
                {
                    return typeof(AttackBehaviourBase);
                }
            }

            // Reproduce
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
            {
                ReproduceBehaviourBase reproduceBehaviour = (ReproduceBehaviourBase)entityModule.GetBehaviourOfType(typeof(ReproduceBehaviourBase));
                if (reproduceBehaviour.CanExecute(entity, entityOnPosition))
                {
                    return typeof(ReproduceBehaviourBase);
                }
            }

            // Eat
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Plant)
            {
                EatBehaviourBase eatBehaviour = (EatBehaviourBase)entityModule.GetBehaviourOfType(typeof(EatBehaviourBase));
                if (eatBehaviour.CanExecute(entity, entityOnPosition))
                {
                    return typeof(EatBehaviourBase);
                }
            }

            // Gather 
            if (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Plant)
            {
                GatherBehaviourBase gatherBehaviour = (GatherBehaviourBase)entityModule.GetBehaviourOfType(typeof(GatherBehaviourBase));
                if (gatherBehaviour.CanExecute(entity, entityOnPosition))
                {
                    return typeof(GatherBehaviourBase);
                }
            }

            // Tame - old code, Humans wont be here
            if (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Animal)
            {
                TameBehaviourBase tameBehaviour = (TameBehaviourBase)entityModule.GetBehaviourOfType(typeof(TameBehaviourBase));
                if (tameBehaviour.CanExecute(entity, entityOnPosition))
                {
                    return typeof(TameBehaviourBase);
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

        private void HandleUpdateWorldEntityStateMessage(TcpClient client, UserInteractionMessage message)
        {
            WorldEntityDTO human = message.HumanEntity;
            WorldEntityDTO? other = message.OtherEntity;

            WorldEntity? humanEntity = entities.Where(e => e.Id == human.Id).FirstOrDefault();
            if (humanEntity == null)
            {
                throw new Exception($"Human entity ({human.Id}) not found in lobby.");
            }

            if (moduleService.GetModuleById(humanEntity.ModuleID)?.Type != EntityTypeEnum.Human)
            {
                throw new Exception($"Entity ({human.Id}) is not a human.");
            }

            SimulateHumanEntityUpdate(human, other);
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

        /// <inheritdoc/>
        public Guid AddClient(TcpClient client, string username)
        {
            lock (clients)
            {
                if (clients.Contains(client))
                {
                    Log("Client already in lobby.", LogLevelEnum.Warning);
                    return Guid.Empty;
                }
                clients.Add(client);
            }

            WorldEntity userEntity = WorldEntity.CreateWorldEntity(username, 0, new EntityState(new SharedLibrary.Helpers.Position2D(0, 0))); //TODO: Add moduleID for human entity.
            AddWorldEntity(userEntity);

            return userEntity.Id;
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

            return true;
        }
        
        public bool AddAllowedModule(int moduleId)
        {
            Module? module = moduleService.GetModuleById(moduleId);
            lock (allowedModulesIDs)
            {
                if (allowedModulesIDs.Contains(moduleId))
                {
                    Log($"ModuleDTO {module?.Name} already allowed in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                allowedModulesIDs.Add(moduleId);
                return true;
            }
        }

        /*// What do we expect here? Just remove in future or present?
        public bool RemoveAllowedModule(int moduleId)
        {
            ModuleDTO? module = moduleService.GetModuleById(moduleId);
            lock (allowedModulesIDs)
            {
                if (!allowedModulesIDs.Contains(moduleId))
                {
                    Log($"ModuleDTO {module?.Name} already isn't allowed in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                allowedModulesIDs.Remove(moduleId);
                return true;
            }
        }*/

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

                bool moduleLoaded = allowedModulesIDs.Any(id => (id == entity.ModuleID));
                if (!moduleLoaded)
                {
                    Log($"Entity's {entity.Id} module is not allowed in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                entities.Add(entity);
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
                return true;
            }
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
