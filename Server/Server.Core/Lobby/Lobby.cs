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
using Server.Core.Behaviours.MoveBehaviour;
using Server.Core.Behaviours.ReproduceBehaviour;
using SharedLibrary.Helpers;
using System.Runtime.InteropServices;

namespace Server.Core.Lobby
{
    /// <summary>
    /// Class representing a game lobby.
    /// </summary>
    public class Lobby : ILobby
    {
        /// <inheritdoc/>
        public int LobbyId { get; init; }

        /// <inheritdoc/>
        public string Name { get; init; }

        /// <inheritdoc/>
        public int MapID { get; init; }

        /// <inheritdoc/>
        public int MaxPlayers { get; init; }

        /// <summary>
        /// Static event used for logging within the lobby.
        /// </summary>
        public static event EventHandler<OnLogEventArgs>? OnLog;

        /// <inheritdoc/>
        public event Action OnLobbyClosed = delegate { };

        
        private readonly List<WorldEntity> entities;                        // All entities in the lobby
        private readonly Dictionary<(int, int), WorldEntity?> entitiesMap;  // Fast lookup by position
        private readonly Dictionary<Guid, WorldEntity> entitiesId;          // Fast lookup by ID
        private readonly List<WorldEntity> updatedEntitiesToPublish;        // Entities that have updates to be sent to clients

        private readonly IModuleService moduleService;
        private readonly List<int> allowedModulesIDs;
        private readonly Dictionary<TcpClient, WorldEntity> clients;

        private readonly bool[][] walkableTiles; // Can be walked on
        private readonly bool[][] fertileTiles;  // Plants can be placed on

        // Locks for thread safety
        private readonly object entitiesLock = new object();
        private readonly object entitiesMapLock = new object();
        private readonly object entitiesIdLock = new object();
        private readonly object entitiesToUpdateLock = new object();
        private readonly object clientsLock = new object();
        private readonly object allowedModulesLock = new object();

        private bool running;

        private readonly int entitiesPerGroup = (int)Math.Ceiling((double)LobbyParams.NUM_INITIAL_ENTITIES / LobbyParams.INITIAL_NUMBER_OF_GROUPS); // Number of entities to update per update
        private int currentGroupIndex = 0; 
        private int totalCycles = 1; //Starts from 1 so we don't reduce health/hunger on first update


        #region Constructors

        /// <summary>
        /// Static factory method used for creating Lobby objects.
        /// </summary>
        /// <param name="id"> ID of the lobby. </param>
        /// <param name="name"> Name of the lobby. </param>
        /// <param name="maxPlayers"> Max allowed number of players in lobby. </param>
        /// <param name="mapId"> ID of the map used in lobby. </param>
        /// <param name="walkableTiles"> Water walkableTiles. </param>
        /// <param name="fertileTiles"> Fertile walkableTiles for plants. </param>
        /// <param name="moduleIDs"> List of allowed modules' IDs. </param>
        /// <param name="moduleService"> IModuleService instance. </param>
        /// <returns> Created Lobby object </returns>
        public static Lobby CreateLobby(int id, string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IEnumerable<int> moduleIDs, IModuleService moduleService)
        {
            Lobby lobby = new Lobby(id, name, maxPlayers, mapId, walkableTiles, fertileTiles, moduleService);

            foreach (int mId in moduleIDs)
            {
                Module? module = moduleService.GetModuleById(mId);
                if (module == null)
                {
                    continue;
                }

                lobby.AddAllowedModule(mId);
            }

            lobby.InitializeWorldEntities();

            return lobby;
        }

        /// Private constructor.
        private Lobby(int id, string name, int maxPlayers, int mapId, bool[][] walkableTiles, bool[][] fertileTiles, IModuleService moduleService)
        {
            LobbyId = id;
            Name = name;
            MaxPlayers = maxPlayers;
            MapID = mapId;

            this.walkableTiles = walkableTiles;
            this.fertileTiles = fertileTiles;
            this.moduleService = moduleService;

            entities = new List<WorldEntity>(LobbyParams.NUM_INITIAL_ENTITIES);
            entitiesMap = new Dictionary<(int, int), WorldEntity?>(walkableTiles[0].Length * walkableTiles.Length);
            entitiesId = new Dictionary<Guid, WorldEntity>(LobbyParams.NUM_INITIAL_ENTITIES);
            updatedEntitiesToPublish = new List<WorldEntity>(LobbyParams.NUM_INITIAL_ENTITIES);

            allowedModulesIDs = new List<int>(20);
            clients = new Dictionary<TcpClient, WorldEntity>(maxPlayers);

            running = true;

            for (int x = 0; x < walkableTiles.Length; x++)
            {
                for (int y = 0; y < walkableTiles[x].Length; y++)
                {
                    entitiesMap[(x, y)] = null;
                }
            }
        }

        #endregion


        #region ILobby Implementation

        /// <inheritdoc/>
        public void Run()
        {
            Log($"Lobby {LobbyId} started.", LogLevelEnum.Info);

            try
            {
                while (running)
                {
                    Task.Delay((int)((1 / LobbyParams.LOBBY_UPDATES_PER_SECOND) * 1000)).Wait();
                    PublishWorldState();
                }
            }
            catch (Exception ex)
            {
                Log(ex, LogLevelEnum.Critical);
            }

            OnLobbyClosed?.Invoke();
        }

        /// <inheritdoc/>
        public bool IsPositionFree(Position2D position)
        {
            lock (entitiesMapLock)
            {
                if (entitiesMap.ContainsKey((position.X, position.Y)))
                {
                    return entitiesMap[(position.X, position.Y)] == null;
                }
            }

            // Out of bounds
            return false; 
        }

        /// <inheritdoc/>
        public Guid AddClient(TcpClient client, string username)
        {
            WorldEntity userEntity = WorldEntity.CreateWorldEntity(username, moduleService.GetHumanModuleId(),
                new EntityState(new Position2D(1, 1), ModulePropertiesLimits.MAX_MAX_HEALTH, ModulePropertiesLimits.MAX_MAX_HUNGER), this);

            lock (clientsLock)
            {
                if (clients.Count >= MaxPlayers)
                {
                    Log("Lobby is full.", LogLevelEnum.Warning);
                    return Guid.Empty;
                }

                if (clients.ContainsKey(client))
                {
                    Log("Client already in lobby.", LogLevelEnum.Warning);
                    return Guid.Empty;
                }

                if (!AddWorldEntity(userEntity))
                {
                    Log("Failed to add user entity to lobby.", LogLevelEnum.Error);
                    return Guid.Empty;
                }

                clients.Add(client, userEntity);
            }

            return userEntity.Id;
        }

        /// <inheritdoc/>
        public bool RemoveClient(TcpClient client)
        {
            lock (clientsLock)
            {
                if (!clients.TryGetValue(client, out WorldEntity? clientEntity))
                {
                    Log("Client already not in lobby.", LogLevelEnum.Warning);
                    return false;
                }

                // Set entity state 0 for other clients to handle and remove from lobby
                clientEntity.State.Health = 0;
                clientEntity.State.Hunger = 0;

                DestroyWorldEntity(clientEntity);
                clients.Remove(client);

                if (clients.Count == 0)
                {
                    Log("No clients left in lobby. Closing lobby.", LogLevelEnum.Info);
                    running = false;
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public bool AddAllowedModule(int moduleId)
        {
            Module? module = moduleService.GetModuleById(moduleId);
            if (module == null)
            {
                Log($"Module with ID={moduleId} not found. Cannot add to allowed modules in lobby {LobbyId}.", LogLevelEnum.Warning);
                return false;
            }

            lock (allowedModulesLock)
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
            if (entity == null)
            {
                return false;
            }

            lock (allowedModulesLock)
            {
                if (!allowedModulesIDs.Contains(entity.ModuleID))
                {
                    Log($"Entity's {entity.Id} module is not allowed in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
            }

            if (moduleService.GetModuleById(entity.ModuleID)?.Type != EntityTypeEnum.Human && !IsPositionFree(entity.State.Position))
                return false;

            lock (entitiesLock)
            {
                if (entities.Contains(entity))
                {
                    Log($"Entity {entity.Id} already exists in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                entities.Add(entity);
            }
            lock (entitiesMapLock)
            {
                entitiesMap[(entity.State.Position.X, entity.State.Position.Y)] = entity;
            }
            lock (entitiesIdLock)
            {
                entitiesId.Add(entity.Id, entity);
            }
            lock (entitiesToUpdateLock)
            {
                if(!updatedEntitiesToPublish.Contains(entity))
                {
                    updatedEntitiesToPublish.Add(entity);
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public bool DestroyWorldEntity(WorldEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            lock (entitiesLock)
            {
                if (!entities.Contains(entity))
                {
                    Log($"Entity {entity.Id} does not exist in lobby {LobbyId}.", LogLevelEnum.Warning);
                    return false;
                }
                entities.Remove(entity);
            }
            lock (entitiesMapLock)
            {
                entitiesMap[(entity.State.Position.X, entity.State.Position.Y)] = null;
            }
            lock (entitiesIdLock)
            {
                entitiesId.Remove(entity.Id);
            }
            lock (entitiesToUpdateLock)
            {
                if (!updatedEntitiesToPublish.Contains(entity))
                {
                    updatedEntitiesToPublish.Add(entity);
                }
            }

            return true;
        }

        /// <inheritdoc/>
        public LobbyDTO ToDTO()
        {
            lock (entitiesLock)
            {
                return new LobbyDTO
                (
                    LobbyId,
                    Name,
                    MaxPlayers,
                    clients.Count,
                    MapID,
                    allowedModulesIDs,
                    entities.Select(e => e.ToDTO()).ToList()
                );
            }
        }

        /// <inheritdoc/>
        public bool IsClientPresent(TcpClient client)
        {
            lock (clientsLock)
            {
                return client != null && clients.ContainsKey(client);
            }

        }

        /// <inheritdoc/>
        public void HandleClientMessage(TcpClient client, MessageBase message)
        {
            if (message == null)
            {
                Log("Received null message from client.", LogLevelEnum.Warning);
                return;
            }

            lock (clientsLock)
            {
                if (client == null || !clients.Keys.Contains(client))
                {
                    return;
                }
            }

            switch (message.MessageType)
            {
                case MessageTypeEnum.UserInteraction:
                    WorldEntityDTO human = ((UserInteractionMessage)message).HumanEntity;
                    WorldEntityDTO? other = ((UserInteractionMessage)message).OtherEntity;
                    SimulateHumanEntityUpdate(human, other);
                    break;

                case MessageTypeEnum.InfoMessage:
                    HandleInfoMessage(client, (InfoMessage)message);
                    break;

                default:
                    HandleUnsupportedMessageType(client, message);
                    break;
            }
        }

        #endregion


        #region Helpers

        /// Creates initial world entities in the lobby.
        private void InitializeWorldEntities()
        {
            List<Module> modules;
            lock (allowedModulesLock)
            {
                modules = allowedModulesIDs.Select(id => moduleService.GetModuleById(id))
                                                    .Where(m => m != null)
                                                    .Cast<Module>()
                                                    .ToList()!;
            }

            int modulesCount = modules.Count;
            
            if (modulesCount <= 1)
            {
                Log("No allowed modules in lobby to create world entities.", LogLevelEnum.Warning);
                return;
            }
            
            Log("Initializing world entities...", LogLevelEnum.Info);


            for (int i = 0; i < LobbyParams.NUM_INITIAL_ENTITIES; i++)
            {
                Module module = modules[new Random().Next(modulesCount)];

                if (module.Type == EntityTypeEnum.Human)
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
                } while (!walkableTiles[x][y] || !IsPositionFree(new Position2D(x, y)) || (module.Type == EntityTypeEnum.Plant && !fertileTiles[x][y]));

                if (attemptsLeft <= 0)
                {
                    continue;
                }

                WorldEntity ent = WorldEntity.CreateWorldEntity($"[{i}]_{module.Name}", module.ID, new EntityState(
                        new Position2D(x, y), module.MaxHealth, module.MaxHunger
                    ), this);

                AddWorldEntity(ent);
            }
        }

        /// Publishes the current world state to all connected clients.
        private void PublishWorldState()
        {
            int startIndex = 0, endIndex = 0;
            lock (entitiesLock)
            {
                if (entities.Count == 0)
                {
                    Log("No entities to update in lobby.", LogLevelEnum.Info);
                    return;
                }

                startIndex = Math.Min(currentGroupIndex * entitiesPerGroup, entities.Count - 1); //Inclusive
                endIndex = Math.Min(startIndex + entitiesPerGroup, entities.Count - 1); //Inclusive

                if (endIndex >= entities.Count - 1)
                {
                    totalCycles++;
                    currentGroupIndex = 0;
                }
                else
                {
                    currentGroupIndex++;
                }
            }

            UpdateWorldState(startIndex, endIndex);

            WorldStateMessage worldStateMessage;
            lock (entitiesToUpdateLock)
            {
                worldStateMessage = new WorldStateMessage(updatedEntitiesToPublish.Select(e => e.ToDTO()).ToList());
                updatedEntitiesToPublish.Clear();
            }

            lock (clientsLock)
            {
                foreach (var clientPair in clients)
                {
                    _ = MessageManager.SendMessageAsync(clientPair.Key, worldStateMessage);
                }
            }
        }

        /// Updates the world state by simulating non-human entities.
        private void UpdateWorldState(int startIndex, int endIndex)
        {
            lock (entitiesLock)
            {
                if (startIndex < 0 || endIndex >= entities.Count || startIndex > endIndex)
                {
                    Log(new ArgumentOutOfRangeException($"Invalid start ({startIndex}) or end ({endIndex}) index for updating world state."), LogLevelEnum.Error);
                    return;
                }
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                WorldEntity entity;
                lock (entitiesLock)
                {
                    if (i >= entities.Count) //In case entities were removed during the update, so the count is always lower than index
                    {
                        break;
                    }

                    entity = entities[i];
                }

                lock (entitiesToUpdateLock)
                {
                    if (!updatedEntitiesToPublish.Contains(entity))
                    {
                        updatedEntitiesToPublish.Add(entity);
                    }
                }

                Module? entityModule = moduleService.GetModuleById(entity.ModuleID);

                if (entityModule == null)
                {
                    Log($"Entity's {entity.Id} module not found in lobby {LobbyId}.", LogLevelEnum.Warning);
                    continue;
                }

                if (entity.State.Health <= 0)
                {
                    entity.Die(moduleService);
                    i--;

                    continue;
                }

                if (totalCycles % LobbyParams.CYCLES_PER_STATS_CHANGE == 0 && entityModule.Type != EntityTypeEnum.Plant)
                {
                    if (entity.State.Hunger > 0) // Deplete hunger
                    {
                        entity.State.Hunger -= LobbyParams.HUNGER_CHANGE;
                    }
                    else if (entity.State.Health > 0) // Deplete health if no hunger
                    {
                        entity.State.Health -= LobbyParams.HEALTH_CHANGE;
                    }

                    if (entity.State.Hunger > 0 && entity.State.Health < entityModule.MaxHealth) // Regenerate health if has hunger
                    {
                        entity.State.Health += LobbyParams.HEALTH_CHANGE;
                    }
                }

                if (entity.State.InteractionCooldownLeft > 0)
                {
                    entity.State.InteractionCooldownLeft--;
                    continue;
                }

                entity.State.LastInteractionName = String.Empty;

                if (entityModule.Type == EntityTypeEnum.Human)
                {
                    continue;
                }

                int stepX = 0, stepY = 0;
                lock (entitiesLock)
                {
                    (stepX, stepY) = ((MoveBehaviourBase)entityModule.GetBehaviourOfType(InteractionTypeEnum.Move)).GetNextMovement(entity, CollectionsMarshal.AsSpan(entities));
                }

                var nextState = new EntityState(entity.State);
                nextState.Position.X += stepX;
                nextState.Position.Y += stepY;

                nextState.Position.X = Math.Clamp(nextState.Position.X, 0, walkableTiles.Length - 1);
                nextState.Position.Y = Math.Clamp(nextState.Position.Y, 0, walkableTiles[0].Length - 1);

                entity.State.LastMovementVector = new Position2D(stepX, stepY);

                SimulateNonHumanEntityUpdate(entity, nextState);
            }
        }

        /// Simulates the update of a non-human entity based on its new state (position).
        private void SimulateNonHumanEntityUpdate(WorldEntity entity, EntityState newState)
        {
            if (entity == null)
            {
                Log(new ArgumentNullException(nameof(entity), "Entity cannot be null."), LogLevelEnum.Error);
                return;
            }
            if (newState == null)
            {
                Log(new ArgumentNullException(nameof(entity), "New state cannot be null."), LogLevelEnum.Error);
                return;
            }

            Module? entityModule = moduleService.GetModuleById(entity.ModuleID);
            if (entityModule == null)
            {
                Log($"Entity's {entity.Id} module not found in lobby {LobbyId}.", LogLevelEnum.Error);
                return;
            }

            WorldEntity? targetEntity;
            lock (entitiesMapLock)
            {
                if (entityModule.Type == EntityTypeEnum.Plant)
                {
                    entitiesMap[(entity.State.Position.X, entity.State.Position.Y)] = entity;
                }

                targetEntity = entitiesMap[(newState.Position.X, newState.Position.Y)];
            }

            // If we change position, there is a possible new interaction. Or we are a plant (to handle growth or other plant-specific behaviour)
            bool shouldCheckInteraction = (entity.State.InteractionCooldownLeft == 0 && (entityModule.Type == EntityTypeEnum.Plant || !entity.State.Position.Equals(newState.Position)));
            if (!shouldCheckInteraction) return;

            InteractionTypeEnum interactionType = GetInteractionType(entity, newState);
            if (interactionType is InteractionTypeEnum.None) return;

            IBehaviour behaviour = entityModule.GetBehaviourOfType(interactionType);

            // Distinction in case when we need to add custom parameters
            int cooldownLeft = LobbyParams.InteractionCooldownInCycles(entities.Count, interactionType);
            switch (interactionType)
            {
                case InteractionTypeEnum.Move:
                    behaviour.Execute(entity, null, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position },
                        { CustomBehaviourParams.ENTITIES_MAP_PARAM, entitiesMap },
                        { CustomBehaviourParams.ENTITIES_MAP_LOCK_PARAM, entitiesMapLock } 
                    });

                    entity.State.InteractionCooldownLeft = cooldownLeft;
                    entity.State.LastInteractionName = nameof(MoveBehaviourBase);

                    break;

                case InteractionTypeEnum.Attack:
                    if (entity.State.LastAttackedEntityId != Guid.Empty)
                    {
                        lock (entitiesIdLock)
                        {
                            if (entitiesId.TryGetValue(entity.State.LastAttackedEntityId, out WorldEntity? target))
                            {
                                if (target != null && behaviour.CanExecute(entity, target, moduleService))
                                {
                                    targetEntity = target;
                                }
                            }
                        }
                    }

                    if (targetEntity != null)
                    {
                        behaviour.Execute(entity, targetEntity, ModuleService.Instance);

                        entity.State.InteractionCooldownLeft = cooldownLeft;
                        targetEntity!.State.InteractionCooldownLeft = cooldownLeft;

                        entity.State.LastInteractionName = nameof(AttackBehaviourBase);
                        targetEntity!.State.LastInteractionName = nameof(AttackBehaviourBase);
                    }

                    break;

                case InteractionTypeEnum.Eat:
                    behaviour.Execute(entity, targetEntity!, ModuleService.Instance);

                    entity.State.InteractionCooldownLeft = cooldownLeft;
                    targetEntity!.State.InteractionCooldownLeft = cooldownLeft;

                    entity.State.LastInteractionName = "Undefined animation interaction";
                    targetEntity!.State.LastInteractionName = "Undefined animation interaction";

                    break;

                case InteractionTypeEnum.Reproduce:
                    behaviour.Execute(entity, targetEntity, ModuleService.Instance, new Dictionary<string, object>{
                    { CustomBehaviourParams.LOBBY_PARAM, this },
                    { CustomBehaviourParams.MAP_FERTILE_PARAM, fertileTiles },
                    { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles   }
                });

                    if (targetEntity != null)
                    {
                        targetEntity.State.InteractionCooldownLeft = cooldownLeft;
                        targetEntity.State.LastInteractionName = nameof(ReproduceBehaviourBase);
                    }
                    entity.State.InteractionCooldownLeft = cooldownLeft;
                    entity.State.LastInteractionName = nameof(ReproduceBehaviourBase);

                    break;

                default:
                    Log($"Undefined interaction type {interactionType} for entity {entity.Id}.", LogLevelEnum.Error);
                    break;
            }
        }

        /// Updates the state of a human entity and optionally another entity. Based on data received from client and treated as delta from last update.
        private void SimulateHumanEntityUpdate(WorldEntityDTO human, WorldEntityDTO? other)
        {
            WorldEntity? entHuman;
            WorldEntity? entOther;

            lock (entitiesIdLock)
            {
                entHuman = entitiesId[human.Id];
            }

            if (entHuman == null)
            {
                Log(new ArgumentNullException(nameof(entHuman), "Entity not found"), LogLevelEnum.Error);
                return;
            }
            if (entHuman.ModuleID != moduleService.GetHumanModuleId())
            {
                Log(new ArgumentNullException(nameof(entHuman), "Entity not of human type"), LogLevelEnum.Error);
                return;
            }

            lock (entitiesMapLock)
            {
                entitiesMap[(entHuman.State.Position.X, entHuman.State.Position.Y)] = null;
                entHuman.UpdateStateWithDelta(human.State.Position, human.State.Health, human.State.Hunger);
                entitiesMap[(entHuman.State.Position.X, entHuman.State.Position.Y)] = entHuman;
            }

            lock (entitiesToUpdateLock)
            {
                if (!updatedEntitiesToPublish.Contains(entHuman))
                {
                    updatedEntitiesToPublish.Add(entHuman);
                }
            }

            if (other == null)
            {
                return;
            }

            lock (entitiesIdLock)
            {
                try
                {
                    entOther = entitiesId[other.Id];
                }
                catch (Exception ex)
                {
                    Log(ex.Message, LogLevelEnum.Error);
                    return;
                }
            }

            if (entOther == null)
            {
                Log("Other entity is null. It does not exist.", LogLevelEnum.Error);
                return;
            }

            lock (entitiesMapLock)
            {
                entitiesMap[(entOther.State.Position.X, entOther.State.Position.Y)] = null;
                entOther.UpdateStateWithDelta(other.State.Position, other.State.Health, other.State.Hunger);

                if (entOther.State.Health > 0)
                {
                    entitiesMap[(entOther.State.Position.X, entOther.State.Position.Y)] = entOther;
                }
            }

            lock (entitiesToUpdateLock)
            {
                if(!updatedEntitiesToPublish.Contains(entOther))
                {
                    updatedEntitiesToPublish.Add(entOther);
                }
            }
        }


        /// Determines the type of interaction that will occur when an entity moves to a new state.
        private InteractionTypeEnum GetInteractionType(WorldEntity entity, EntityState newState)
        {
            Module? entityModule = moduleService.GetModuleById(entity.ModuleID);
            if (entityModule == null)
            {
                Log($"Entity's {entity.Id} module [{entity.ModuleID}] not found in lobby {LobbyId}.", LogLevelEnum.Error);
                return InteractionTypeEnum.None;
            }

            EntityTypeEnum entityType = entityModule.Type;
            WorldEntity? entityOnPosition;
            int entitiesCount;

            lock (entitiesLock)
            {
                entitiesCount = entities.Count;
            }
            
            // We refactored this so that humans don't use this method, the clients send the human updates directly.
            if (entityType == EntityTypeEnum.Human) return InteractionTypeEnum.None;

            lock (entitiesMapLock)
            {
                entityOnPosition = entitiesMap[(newState.Position.X, newState.Position.Y)];
            }

            // Plants reproduce by themselves - and do only this.
            if (entityType == EntityTypeEnum.Plant)
            {
                if (entitiesCount < LobbyParams.MAX_ENTITIES && entityModule.GetBehaviourOfType(InteractionTypeEnum.Reproduce)
                    .CanExecute(entity, entityOnPosition, ModuleService.Instance, new Dictionary<string, object>()
                    {
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles }
                    })
                )
                {
                    return InteractionTypeEnum.Reproduce;
                }
                return InteractionTypeEnum.None;
            }

            if (entity.State.LastAttackedEntityId != Guid.Empty)
            {
                AttackBehaviourBase attackBehaviour = (AttackBehaviourBase)entityModule.GetBehaviourOfType(InteractionTypeEnum.Attack);
                WorldEntity? lastAttackedEntity;
                lock (entitiesIdLock)
                {
                    entitiesId.TryGetValue(entity.State.LastAttackedEntityId, out lastAttackedEntity);
                }

                if (lastAttackedEntity != null &&
                    lastAttackedEntity.State.Health > 0 &&
                    lastAttackedEntity.State.InteractionCooldownLeft == 0 &&
                    Math.Abs(lastAttackedEntity.State.Position.X - entity.State.Position.X) <= 1 &&
                    Math.Abs(lastAttackedEntity.State.Position.Y - entity.State.Position.Y) <= 1 &&
                    attackBehaviour.CanExecute(entity, lastAttackedEntity, ModuleService.Instance))
                {
                    return InteractionTypeEnum.Attack;
                }

                entity.State.LastAttackedEntityId = Guid.Empty;
            }

            if (entityOnPosition == null)
            {
                if (entityModule.GetBehaviourOfType(InteractionTypeEnum.Move)
                    .CanExecute(entity, null, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_WALKABLE_PARAM, walkableTiles },
                        { CustomBehaviourParams.NEW_POS_PARAM, newState.Position },
                        { CustomBehaviourParams.LOBBY_PARAM, this }
                    })
                )
                {
                    return InteractionTypeEnum.Move;
                }
                return InteractionTypeEnum.None;
            }

            EntityTypeEnum? targetType = moduleService.GetModuleById(entityOnPosition.ModuleID)?.Type;
            if (targetType == null)
            {
                Log($"Target's [{entityOnPosition.Id}] module [{entityOnPosition.ModuleID}] not found.", LogLevelEnum.Error);
                return InteractionTypeEnum.None;
            }

            // Reproduce - Animals reproduce with animals
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal)
            {
                ReproduceBehaviourBase reproduceBehaviour = (ReproduceBehaviourBase)entityModule.GetBehaviourOfType(InteractionTypeEnum.Reproduce);
                if (entitiesCount < LobbyParams.MAX_ENTITIES && reproduceBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance, new Dictionary<string, object>{
                        { CustomBehaviourParams.MAP_FERTILE_PARAM, fertileTiles }
                    }))
                {
                    return InteractionTypeEnum.Reproduce;
                }
            }

            // Attack - Humans wont be here
            if ((entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Human) ||
                (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Animal))
            {
                AttackBehaviourBase attackBehaviour = (AttackBehaviourBase)entityModule.GetBehaviourOfType(InteractionTypeEnum.Attack);
                if (attackBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return InteractionTypeEnum.Attack;
                }
            }


            // Eat
            if (entityType == EntityTypeEnum.Animal && targetType == EntityTypeEnum.Plant)
            {
                EatBehaviourBase eatBehaviour = (EatBehaviourBase)entityModule.GetBehaviourOfType(InteractionTypeEnum.Eat);
                if (eatBehaviour.CanExecute(entity, entityOnPosition, ModuleService.Instance))
                {
                    return InteractionTypeEnum.Eat;
                }
            }

            return InteractionTypeEnum.None;
        }

        #endregion


        #region Handlers

        /// <summary>
        /// Handles info messages from clients, for now just logs a warning as unsupported.
        /// </summary>
        private void HandleInfoMessage(TcpClient client, InfoMessage message)
        {
            Log($"Received unsupported info message of type: {message.MessageType}", LogLevelEnum.Warning);
        }

        /// <summary>
        /// Handles unsupported messages from clients - just logs a warning as unsupported.
        /// </summary>
        private void HandleUnsupportedMessageType(TcpClient client, MessageBase message)
        {
            Log($"Received unsupported message type: {message.MessageType}", LogLevelEnum.Error);
        }

        #endregion


        #region Logging

        /// Logs an exception message with the specified log level.
        private void Log(Exception ex, LogLevelEnum level)
        {
            Log(ex.Message, level);
        }

        /// Logs a message with the specified log level.
        private void Log(string message, LogLevelEnum level)
        {
            OnLogEventArgs args = new OnLogEventArgs(message, level);

            OnLog?.Invoke(this, args);
        }

        #endregion

    }

}