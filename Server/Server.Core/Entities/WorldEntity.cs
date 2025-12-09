using Server.Core.Exceptions;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
    /// <summary>
    /// Class representing an entity in the game world.
    /// </summary>
    public class WorldEntity
    {
        /// <summary>
        /// ID of the world entity.
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Name of the world entity.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// ID of the module the entity belongs to.
        /// </summary>
        public int ModuleID { get; init; }

        /// <summary>
        /// State of the world entity.
        /// </summary>
        public EntityState State { get; init; }

        /// <summary>
        /// Lobby the entity belongs to.
        /// </summary>
        public ILobby Lobby { get; init; }

        /// <summary>
        /// Creates a new instance of WorldEntity.
        /// </summary>
        /// <param name="name"> Optional name of the world entity. </param>
        /// <param name="moduleId"> ID of the module the entity belongs to. </param>
        /// <param name="state"> Initial state of the world entity. </param>    
        /// <param name="lobby"> Lobby the entity belongs to. </param>
        /// <returns> New instance of WorldEntity. </returns>
        public static WorldEntity CreateWorldEntity(string? name, int moduleId, EntityState state, ILobby lobby)
        {
            return new WorldEntity(name, moduleId, state, lobby);
        }

        /// Private constructor.
        private WorldEntity(string? name, int moduleId, EntityState state, ILobby lobby) 
        {
            Name = name;
            Id = Guid.NewGuid();
            ModuleID = moduleId;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
            Lobby = lobby ?? throw new ArgumentNullException(nameof(lobby), "Lobby cannot be null.");
        }

        /// <summary>
        /// Updates the entity's state with a new state.
        /// </summary>
        /// <param name="newState"> New state to update the entity with. </param>
        public void UpdateState(EntityState newState)
        {
            Module module = ModuleService.Instance.GetModuleById(ModuleID) ?? throw new ModuleNotFoundException($"Module with ID {ModuleID} not found for entity {Id}");

            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            State.Health = newState.Health;
            State.Position = new (newState.Position);
            State.Hunger = newState.Hunger;
            State.InteractionCooldownLeft = newState.InteractionCooldownLeft;

            if (State.Health <= 0 && module.Type != EntityTypeEnum.Human)
            {
                Die(ModuleService.Instance);
            }
        }

        /// <summary>
        /// Updates the entity's state with deltas.
        /// </summary>
        /// <param name="positionDelta"> Change in position. </param>
        /// <param name="healthDelta"> Change in health. </param>
        /// <param name="hungerDelta"> Change in hunger. </param>
        public void UpdateStateWithDelta(Position2D positionDelta, int healthDelta, int hungerDelta)
        {
            Module module = ModuleService.Instance.GetModuleById(ModuleID) ?? throw new ModuleNotFoundException($"Module with ID {ModuleID} not found for entity {Id}");
            
            if(!positionDelta.Equals(new Position2D(0,0)) && module.Type != EntityTypeEnum.Human)
            {
                Console.WriteLine("Non human entity changed position after client update.");
            }

            State.Position += positionDelta;
            State.Health += healthDelta;
            State.Hunger += hungerDelta;

            State.Health = Math.Clamp(State.Health, 0, module.MaxHealth);
            State.Hunger = Math.Clamp(State.Hunger, 0, module.MaxHunger);

            if (State.Health <= 0 && module.Type != EntityTypeEnum.Human)
            {
                Die(ModuleService.Instance);
            }
        }

        /// <summary>
        /// Dies the entity.
        /// </summary>
        public void Die(IModuleService moduleService)
        {
            Module? module = moduleService.GetModuleById(ModuleID) ?? throw new ModuleNotFoundException($"Module with ID {ModuleID} not found for entity {Id}");
            this.State.Health = 0;
            this.State.Hunger = 0;

            if (module.Type == EntityTypeEnum.Human)
            {
                // Client handle human entity death
            }
            else
            {
                Lobby.DestroyWorldEntity(this);
            }
        }

        /// <summary>
        /// Creates a DTO representation of the world entity.
        /// </summary>
        /// <returns> DTO of the world entity. </returns>
        public WorldEntityDTO ToDTO()
        {
            return new WorldEntityDTO (this.Name, this.Id, this.State.ToDTO(), this.ModuleID);
        }

    }

}