using System.Net.Sockets;
using Server.Core.Modules;
using SharedLibrary.Logging;
using SharedLibrary.Messages;
using SharedLibrary.DTOs.EntitiesDTO;
using Server.Core.Behaviours;
using Server.Core.Helpers;
using Server.Core.Services;
using SharedLibrary.DTOs.LobbyDTO;
using Server.Core.Behaviours.AttackBehaviour;
using Server.Core.Behaviours.EatBehaviour;
using Server.Core.Behaviours.GatherBehaviour;
using Server.Core.Behaviours.MoveBehaviour;
using Server.Core.Behaviours.TameBehaviour;
using Server.Core.Behaviours.ReproduceBehaviour;
using SharedLibrary.Helpers;

namespace Server.Core.Lobby
{
    public class Lobby : ILobby
    {
        /// <inheritdoc/>
        public int LobbyId { get; init; }
        
        /// <inheritdoc/>
        public string Name { get; init; }
        
        /// <inheritdoc/>
        public int MapID{ get; init; }
        
        /// <inheritdoc/>
        public int MaxPlayers { get; init; }


        /// <summary>
        /// Static event for logging within the lobby.
        /// </summary>
        public static event EventHandler<OnLogEventArgs>? OnLog;
        
        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;

        private readonly IModuleService moduleService;
        private readonly Dictionary<TcpClient, WorldEntity> clients;
        private readonly List<WorldEntity> entities;
        private readonly List<int> allowedModulesIDs;
        private readonly bool[][] walkableTiles;

        private bool running;

        public static Lobby CreateLobby(int id, string name, int maxPlayers, int mapId, bool[][] walkableTiles, IEnumerable<int> moduleIDs, IModuleService moduleService)
        {
            Lobby lobby = new Lobby(id, name, maxPlayers, mapId, walkableTiles, moduleService);
            
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

        private Lobby(int id, string name, int maxPlayers, int mapId, bool[][] tiles, IModuleService moduleService)
        {
            LobbyId = id;
            Name = name;
            MaxPlayers = maxPlayers;
            walkableTiles = tiles; 
            this.moduleService = moduleService;

            entities = new List<WorldEntity>(200);
            allowedModulesIDs = new List<int>(20);
            clients = new Dictionary<TcpClient, WorldEntity>(maxPlayers);
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
            const int NUM_INITIAL_ENTITIES = 100;

            IModuleService moduleService = ModuleService.Instance;
            List<Module> modules = (moduleService.GetAllModules().ToList());
            int modulesCount = modules.Count;

            Module module;

            Log("Initializing world entities...", LogLevelEnum.Info);

            // For testing purposes, we create some entities here.
            for (int i = 0; i < NUM_INITIAL_ENTITIES; i++)
            {
                module = modules[new Random().Next(modulesCount)];
                
                if(module.Type == EntityTypeEnum.Human)
                {
                    i--;
                    continue;
                }

                int x, y;
                do
                {
                    x = new Random().Next(walkableTiles.Length);
                    y = new Random().Next(walkableTiles[0].Length);
                } while (!walkableTiles[x][y] || !IsPositionFree(new Position2D(x, y)));

                WorldEntity ent = WorldEntity.CreateWorldEntity($"[{i}]_{module.Name}", module.ID, new EntityState(
                        new Position2D(x, y)
                    ), this);
                
                AddWorldEntity(ent);
            }
        }

        private void PublishWorldState()
        {
            // Simulate all non-human entities
            Module? entityModule;
            EntityState nextState;

            Log("Updating world entities...", LogLevelEnum.Debug);
            for (int i = 0; i < entities.Count; i++) 
            {
                WorldEntity entity = entities[i];

                if(entity.State.InteractionFramesLeft > 0)
                {
                    entity.State.InteractionFramesLeft--;
                    continue;
                }
                entity.State.LastInteractionName = String.Empty;

                entityModule = moduleService.GetModuleById(entity.ModuleID);
                if(entityModule == null)
                {
                    Log($"Entity's {entity.Id} module not found in lobby {LobbyId}.", LogLevelEnum.Warning);
                    continue;
                }

                if (entityModule.Type == EntityTypeEnum.Human)
                {
                    continue;
                }

                var moveBehaviour = entityModule.GetBehaviourOfType(typeof(MoveBehaviourBase));
                nextState = new EntityState(entity.State);

                (int stepX, int stepY) = ((MoveBehaviourBase)moveBehaviour).GetNextMovement(entity);
                entity.State.LastMovementVector = new Position2D(stepX, stepY);

                nextState.Position.X += stepX;
                nextState.Position.Y += stepY;

                SimulateNonHumanEntityUpdate(entity, nextState);
            }

            lock (clients)
            {
                foreach (var clientPair in clients)
                {
                    _ = MessageManager.SendMessageAsync(clientPair.Key, new WorldStateMessage(
                            entities.Select(e => e.ToDTO())
                        ));
                }
            }
        }

        //@FranciszekGwarek do we need these things?
        // ???????????????????????
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

        private void SimulateNonHumanEntityUpdate(WorldEntity entity, EntityState newState)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");
            }
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            Module? entityModule = moduleService.GetModuleById(entity.ModuleID)
                ?? throw new Exception($"Entity's {entity.Id} module not found.");


            // If we change position, there is a possible new interaction. Or we are a plant (to handle growth or other plant-specific behaviour)
            if (entity.State.InteractionFramesLeft == 0 && (entity.State.Position != newState.Position || entityModule.Type == EntityTypeEnum.Plant))
            {
                Type? interactionType = GetInteractionType(entity, newState);
                if (interactionType is null) return;

                WorldEntity? targetEntity = entities.Where(e => e.State.Position == newState.Position)?.FirstOrDefault();
                if(targetEntity == null && interactionType != typeof(MoveBehaviourBase))
                {
                    return;
                }

                IBehaviour behaviour = entityModule.GetBehaviourOfType(interactionType);

                // Distinction in case when we need to add custom parameters
                if (interactionType == typeof(MoveBehaviourBase))
                {
                    behaviour.Execute(entity, null, ModuleService.Instance ,new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_PARAM, walkableTiles   },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position }
                    });

                    entity.State.InteractionFramesLeft = 32;
                    entity.State.LastInteractionName = nameof(MoveBehaviourBase);
                }
                else if (interactionType == typeof(ReproduceBehaviourBase))
                {
                    behaviour.Execute(entity, targetEntity!, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.LOBBY_PARAM, this }
                    });

                    entity.State.InteractionFramesLeft = 80;
                    targetEntity!.State.InteractionFramesLeft = 80;
                    entity.State.LastInteractionName = nameof(ReproduceBehaviourBase);
                }
                else if(interactionType == typeof(AttackBehaviourBase))
                {
                    entity.State.InteractionFramesLeft = 64;
                    targetEntity!.State.InteractionFramesLeft = 64;
                    entity.State.LastInteractionName = nameof(AttackBehaviourBase);
                }
                else
                {
                    behaviour.Execute(entity, targetEntity!, ModuleService.Instance);
                    entity.State.InteractionFramesLeft = 64;
                    targetEntity!.State.InteractionFramesLeft = 64;
                }
            }
        }
        private void SimulateHumanEntityUpdate(WorldEntityDTO human, WorldEntityDTO? other)
        {
            WorldEntity? entHuman = entities.Where(e => e.Id == human.Id)?.FirstOrDefault();
            WorldEntity? entOther = entities.Where(e => e.Id == other?.Id)?.FirstOrDefault();

            if (entHuman == null)
            {
                throw new ArgumentNullException(nameof(entHuman), "Entity not found");
            }

            entHuman.UpdateState(new EntityState(human.State));
            entOther?.UpdateState(new EntityState(other!.State));
        }

        private Type? GetInteractionType(WorldEntity entity, EntityState newState)
        {
            WorldEntity? entityOnPosition = entities.Where(ent => ent.State.Position == newState.Position)?.FirstOrDefault();

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
                if (attackBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(AttackBehaviourBase);
                }
            }

            // Reproduce
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
            {
                ReproduceBehaviourBase reproduceBehaviour = (ReproduceBehaviourBase)entityModule.GetBehaviourOfType(typeof(ReproduceBehaviourBase));
                if (reproduceBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(ReproduceBehaviourBase);
                }
            }

            // Eat
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Plant)
            {
                EatBehaviourBase eatBehaviour = (EatBehaviourBase)entityModule.GetBehaviourOfType(typeof(EatBehaviourBase));
                if (eatBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(EatBehaviourBase);
                }
            }

            // Gather 
            if (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Plant)
            {
                GatherBehaviourBase gatherBehaviour = (GatherBehaviourBase)entityModule.GetBehaviourOfType(typeof(GatherBehaviourBase));
                if (gatherBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(GatherBehaviourBase);
                }
            }

            // Tame - old code, Humans wont be here
            if (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Animal)
            {
                TameBehaviourBase tameBehaviour = (TameBehaviourBase)entityModule.GetBehaviourOfType(typeof(TameBehaviourBase));
                if (tameBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
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
                if(!clients.Keys.Contains(args.Client))
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

            WorldEntity? humanEntity = entities.Where(e => e.Id == human.Id)?.FirstOrDefault();
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
        public bool IsPositionFree(Position2D position)
        {
            foreach (var entity in entities)
            {
                if (entity.State.Position == position)
                {
                    return false;
                }
            }
            return true;
        }

        /// <inheritdoc/>
        public Guid AddClient(TcpClient client, string username)
        {
            WorldEntity userEntity = WorldEntity.CreateWorldEntity(username, moduleService.GetHumanModuleId(), 
                new EntityState(new Position2D(0, 0)), this);
            
            lock (clients)
            {
                if (clients.Keys.Contains(client))
                {
                    Log("Client already in lobby.", LogLevelEnum.Warning);
                    return Guid.Empty;
                }
                clients.Add(client, userEntity);
            }

            AddWorldEntity(userEntity);

            return userEntity.Id;
        }

        public bool RemoveClient(TcpClient client)
        {
            lock (clients)
            {
                if (!clients.Keys.Contains(client))
                {
                    Log("Client already not in lobby.", LogLevelEnum.Warning);
                    return false;
                }
                clients.Remove(client);
                DestroyWorldEntity(clients[client]);
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

                if (!IsPositionFree(entity.State.Position)) return false;

                entities.Add(entity);
                return true;
            }
        }

        public bool DestroyWorldEntity(WorldEntity entity)
        {
            if(entity == null)
            {
                return false;
            }

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

        public LobbyDTO ToDTO()
        {
            return new LobbyDTO
            (
                LobbyId,
                Name,
                MaxPlayers,
                clients.Count,
                MapID,
                allowedModulesIDs
            );
        }

        #endregion

        #region Logging
        private void Log(Exception ex, LogLevelEnum level)
        {
            Log(ex.Message, level);
        }

        private void Log(string message, LogLevelEnum level)
        {
            OnLogEventArgs args = new OnLogEventArgs(message, level);

            OnLog?.Invoke(this, args);
        }
        #endregion
    }
}
