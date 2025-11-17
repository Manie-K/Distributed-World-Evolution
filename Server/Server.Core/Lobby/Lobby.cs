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
using System.Collections.Immutable;

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
        /// Static event used for logging within the lobby.
        /// </summary>
        public static event EventHandler<OnLogEventArgs>? OnLog;

        /// <inheritdoc/>
        public event Action OnLobbyClosed = delegate { };

        /// <summary>
        /// Lobby updates per second.
        /// </summary>
        public const double LOBBY_UPDATES_PER_SECOND = 64;

        private readonly IModuleService moduleService;
        private readonly Dictionary<TcpClient, WorldEntity> clients;
        private readonly List<WorldEntity> entities;
        private readonly List<int> allowedModulesIDs;
        private readonly bool[][] walkableTiles;
        private readonly bool[][] fertileTiles;

        private int entitiesHealthAndHungerUpdateCounter = 0;
        private bool running;

        #region Constructors

        /// <summary>
        /// Static factory method used for creating Lobby objects.
        /// </summary>
        /// <param name="id">ID of the lobby</param>
        /// <param name="name">Name of the lobby</param>
        /// <param name="maxPlayers">Max allowed number of players in lobby</param>
        /// <param name="mapId">ID of the map used in lobby</param>
        /// <param name="walkableTiles">Water walkableTiles</param>
        /// <param name="fertileTiles">Fertile walkableTiles for plants</param>
        /// <param name="moduleIDs">List of allowed modules' IDs</param>
        /// <param name="moduleService">IModuleService instance</param>
        public static Lobby CreateLobby(int id, string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IEnumerable<int> moduleIDs, IModuleService moduleService)
        {
            Lobby lobby = new Lobby(id, name, maxPlayers, mapId, walkableTiles, fertileTiles, moduleService);
            
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

        private Lobby(int id, string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IModuleService moduleService)
        {
            LobbyId = id;
            Name = name;
            MaxPlayers = maxPlayers;
            MapID = mapId;

            this.walkableTiles = walkableTiles; 
            this.fertileTiles = fertileTiles; 
            this.moduleService = moduleService;

            entities = new List<WorldEntity>(200);
            allowedModulesIDs = new List<int>(20);
            clients = new Dictionary<TcpClient, WorldEntity>(maxPlayers);
            running = true;

            Server.OnMessageFromClientReceived += OnMessageFromClientReceived_Delegate;
        }

        #endregion


        #region ILooby Implementation

        /// <inheritdoc/>
        public void Run()
        {
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            while (running)
            {
                Task.Delay((int)((1 / LOBBY_UPDATES_PER_SECOND) * 1000)).Wait();
                PublishWorldState();
            }

            OnLobbyClosed?.Invoke();
            Log($"Lobby {LobbyId} closed.", LogLevelEnum.Info);
        }

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
            if(clients.Count >= MaxPlayers)
            {
                Log("Lobby is full.", LogLevelEnum.Warning);
                return Guid.Empty;
            }

            WorldEntity userEntity = WorldEntity.CreateWorldEntity(username, moduleService.GetHumanModuleId(),
                new EntityState(new Position2D(0, 0), ModulePropertiesLimits.MAX_MAX_HEALTH, ModulePropertiesLimits.MAX_MAX_HUNGER), this);

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

        /// <inheritdoc/>
        public bool RemoveClient(TcpClient client)
        {
            lock (clients)
            {
                if (!clients.Keys.Contains(client))
                {
                    Log("Client already not in lobby.", LogLevelEnum.Warning);
                    return false;
                }
                DestroyWorldEntity(clients[client]);
                clients.Remove(client);
            }

            if(clients.Count == 0)
            {
                Log("No clients left in lobby. Closing lobby.", LogLevelEnum.Info);
                running = false;
            }

            return true;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool DestroyWorldEntity(WorldEntity entity)
        {
            if (entity == null)
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

        /// <inheritdoc/>
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


        #region Helpers

        /// <summary>
        /// Creates initial world entities in the lobby.
        /// </summary>
        private void InitializeWorldEntities()
        {
            IModuleService moduleService = ModuleService.Instance;
            List<Module> modules = (moduleService.GetAllModules().ToList());
            int modulesCount = modules.Count;

            Module module;

            Log("Initializing world entities...", LogLevelEnum.Info);

            // For testing purposes, we create some entities here.

            for (int i = 0; i < LobbyParams.NUM_INITIAL_ENTITIES; i++)
            {
                module = modules[new Random().Next(modulesCount)];
                
                if(module.Type == EntityTypeEnum.Human)
                {
                    i--;
                    continue;
                }

                int attemptsLeft = 100;
                int x = 0, y = 0;
                do 
                {
                    if (attemptsLeft-- <= 0)
                    {
                        Log("Failed to place world entity during initialization after 100 attempts.", LogLevelEnum.Warning);
                        break;
                    }

                    x = new Random().Next(walkableTiles.Length);
                    y = new Random().Next(walkableTiles[0].Length);
                } while (!walkableTiles[x][y] || !IsPositionFree(new Position2D(x, y)));

                if(attemptsLeft <= 0)
                {
                    continue;
                }

                WorldEntity ent = WorldEntity.CreateWorldEntity($"[{i}]_{module.Name}", module.ID, new EntityState(
                        new Position2D(x, y) , module.MaxHealth, module.MaxHunger
                    ), this);
                
                AddWorldEntity(ent);
            }
        }

        /// <summary>
        /// Publishes the current world state to all connected clients.
        /// </summary>
        private void PublishWorldState()
        {
            UpdateWorldState();

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

        /// <summary>
        /// Updates the world state by simulating all non-human entities.
        /// </summary>
        private void UpdateWorldState()
        {
            const int HUNGER_CHANGE = 1;
            const int HEALTH_CHANGE = 1;

            Module? entityModule;
            EntityState nextState;

            entitiesHealthAndHungerUpdateCounter++;
            bool shouldResetCounter = false;

            for (int i = 0; i < entities.Count; i++)
            {
                WorldEntity entity = entities[i];
                entityModule = moduleService.GetModuleById(entity.ModuleID);

                if (entityModule == null)
                {
                    Log($"Entity's {entity.Id} module not found in lobby {LobbyId}.", LogLevelEnum.Warning);
                    continue;
                }

                if (entitiesHealthAndHungerUpdateCounter >= 10 * LOBBY_UPDATES_PER_SECOND)
                {
                    shouldResetCounter = true;
                    if (entity.State.Health <= 0)
                    {
                        entity.Die(moduleService);
                        i--;
                        continue;
                    }
                    else if (entity.State.Hunger > 0)
                    {
                        entity.State.Hunger -= HUNGER_CHANGE;
                    }
                    else if (entity.State.Health > 0)
                    {
                        entity.State.Health -= HEALTH_CHANGE;
                    }

                    if (entity.State.Hunger > 0 && entity.State.Health < entityModule.MaxHealth)
                    {
                        entity.State.Health += HEALTH_CHANGE;
                    }
                }


                if (entity.State.InteractionFramesLeft > 0)
                {
                    entity.State.InteractionFramesLeft--;
                    continue;
                }
                entity.State.LastInteractionName = String.Empty;

                if (entityModule.Type == EntityTypeEnum.Human)
                {
                    continue;
                }


                (int stepX, int stepY) = ((MoveBehaviourBase)entityModule.GetBehaviourOfType(typeof(MoveBehaviourBase))).GetNextMovement(entity, ImmutableList.Create(entities.ToArray()));

                nextState = new EntityState(entity.State);
                nextState.Position.X += stepX;
                nextState.Position.Y += stepY;

                entity.State.LastMovementVector = new Position2D(stepX, stepY);

                SimulateNonHumanEntityUpdate(entity, nextState);
            }

            if (shouldResetCounter)
            {
                entitiesHealthAndHungerUpdateCounter = 0;
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
                        HandleUpdateHumanWorldEntityMessage(client, (UserInteractionMessage)message);
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

        /// <summary>
        /// Simulates the update of a non-human entity based on its new state (position).
        /// </summary>
        /// <param name="entity">Entity to be simulated.</param>
        /// <param name="newState">This entity's new state, with next simulated position.</param>
        /// <exception cref="ArgumentNullException">entity and newState must not be null.</exception>
        /// <exception cref="Exception">entity module must be correct.</exception>
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
            bool shouldCheckInteraction = ((entity.State.Position != newState.Position) || (entityModule.Type == EntityTypeEnum.Plant)) 
                                          && (entity.State.InteractionFramesLeft == 0);

            if (shouldCheckInteraction)
            {
                Type? interactionType = GetInteractionType(entity, newState);
                if (interactionType is null) return;

                WorldEntity? targetEntity = entities.Where(e => e.State.Position == newState.Position)?.FirstOrDefault();
                IBehaviour behaviour = entityModule.GetBehaviourOfType(interactionType);

                // Distinction in case when we need to add custom parameters
                if (interactionType == typeof(MoveBehaviourBase))
                {
                    behaviour.Execute(entity, null, ModuleService.Instance ,new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles   },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position }
                    });

                    entity.State.InteractionFramesLeft = 32;
                    entity.State.LastInteractionName = nameof(MoveBehaviourBase);
                }
                else if (interactionType == typeof(ReproduceBehaviourBase))
                {
                    behaviour.Execute(entity, targetEntity, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.LOBBY_PARAM, this },
                        { CustomBehaviourParams.MAP_FERTILE_PARAM, fertileTiles },
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles   }
                    });

                    if(targetEntity != null)
                    {
                        targetEntity.State.InteractionFramesLeft = 96;
                        targetEntity.State.LastInteractionName = nameof(ReproduceBehaviourBase);
                    }
                    entity.State.InteractionFramesLeft = 96;
                    entity.State.LastInteractionName = nameof(ReproduceBehaviourBase);
                }
                else if(interactionType == typeof(AttackBehaviourBase))
                {
                    behaviour.Execute(entity, targetEntity!, ModuleService.Instance);
                    
                    entity.State.InteractionFramesLeft = 128;
                    targetEntity!.State.InteractionFramesLeft = 128;
                    
                    entity.State.LastInteractionName = nameof(AttackBehaviourBase);
                    targetEntity!.State.LastInteractionName = nameof(AttackBehaviourBase);
                }
                else
                {
                    behaviour.Execute(entity, targetEntity!, ModuleService.Instance);
                    
                    entity.State.InteractionFramesLeft = 128;
                    targetEntity!.State.InteractionFramesLeft = 128;
                    
                    entity.State.LastInteractionName = "Undefined interaction";
                    targetEntity!.State.LastInteractionName = "Undefined interaction";
                }
            }
        }

        /// <summary>
        /// Updates the state of a human entity and optionally another entity. Based on data received from client.
        /// </summary>
        /// <param name="human">Human entity to be changed. Must be human.</param>
        /// <param name="other">Other entity to be changed. Optional.</param>
        /// <exception cref="ArgumentNullException">Human entity can not be null.</exception>
        private void SimulateHumanEntityUpdate(WorldEntityDTO human, WorldEntityDTO? other)
        {
            WorldEntity? entHuman = entities.Where(e => e.Id == human.Id)?.FirstOrDefault();
            WorldEntity? entOther = entities.Where(e => e.Id == other?.Id)?.FirstOrDefault();

            if (entHuman == null)
            {
                throw new ArgumentNullException(nameof(entHuman), "Entity not found");
            }
            if(entHuman.ModuleID != moduleService.GetHumanModuleId())
            {
                throw new ArgumentNullException(nameof(entHuman), "Entity not of human type");
            }

            entHuman.UpdateState(new EntityState(human.State));
            entOther?.UpdateState(new EntityState(other!.State));
        }

        /// <summary>
        /// Determines the type of interaction that will occur when an entity moves to a new state.
        /// </summary>
        /// <param name="entity">Entity to be checked</param>
        /// <param name="newState">Entity's new state</param>
        /// <returns>Type of the interaction, or null if no interaction should occur</returns>
        /// <exception cref="Exception">Modules must be correct</exception>
        private Type? GetInteractionType(WorldEntity entity, EntityState newState)
        {
            Module entityModule = moduleService.GetModuleById(entity.ModuleID) ?? throw new Exception($"Module with ID={entity.ModuleID} not found");
            EntityTypeEnum entityType = entityModule.Type;

            WorldEntity? entityOnPosition = entities.Where(ent => ent.State.Position == newState.Position)?.FirstOrDefault();

            if (entityOnPosition == null)
            {
                if(entityModule.GetBehaviourOfType(typeof(MoveBehaviourBase))
                    .CanExecute(entity, null, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles   },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position },
                        { CustomBehaviourParams.LOBBY_PARAM, this }
                    })
                )
                {
                    return typeof(MoveBehaviourBase);
                }
                return null;
            }

            EntityTypeEnum targetType = moduleService.GetModuleById(entityOnPosition.ModuleID)?.Type ?? throw new Exception($"Module with ID={entity.ModuleID} not found"); ;

            // We refactored this so that humans dont use this method, the clients send the human updates directly
            if (entityType == EntityTypeEnum.Human) return null;

            // Plants reproduce by themselves - and do only this
            if (entityType == EntityTypeEnum.Plant)
            {
                if(entityOnPosition != entity)
                {
                    Log("Plant is on the same position as another entity, which should not happen.", LogLevelEnum.Error);
                }

                if(entityModule.GetBehaviourOfType(typeof(ReproduceBehaviourBase))
                    .CanExecute(entity, entityOnPosition, ModuleService.Instance, new Dictionary<string, object>()
                    {
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
                    })
                )
                {
                    return typeof(ReproduceBehaviourBase);
                }
                return null;
            }

            // Attack - Humans wont be here
            if ((entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal))
            {
                AttackBehaviourBase attackBehaviour = (AttackBehaviourBase)entityModule.GetBehaviourOfType(typeof(AttackBehaviourBase));
                if (attackBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(AttackBehaviourBase);
                }
            }

            // Reproduce - Animals reproduce with animals
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
            {
                ReproduceBehaviourBase reproduceBehaviour = (ReproduceBehaviourBase)entityModule.GetBehaviourOfType(typeof(ReproduceBehaviourBase));
                if (reproduceBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_FERTILE_PARAM, fertileTiles }
                    }))
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

            // Gather - old code, Humans won't be here
            if (entityType == EntityTypeEnum.Human && targetType == EntityTypeEnum.Plant)
            {
                GatherBehaviourBase gatherBehaviour = (GatherBehaviourBase)entityModule.GetBehaviourOfType(typeof(GatherBehaviourBase));
                if (gatherBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return typeof(GatherBehaviourBase);
                }
            }

            // Tame - old code, Humans won't be here
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

        #endregion


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

        private void HandleUpdateHumanWorldEntityMessage(TcpClient client, UserInteractionMessage message)
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
