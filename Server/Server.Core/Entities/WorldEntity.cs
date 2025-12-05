using Server.Core.Exceptions;
using Server.Core.Lobby;
using Server.Core.Modules;
using Server.Core.Services;
using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
    public class WorldEntity
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public int ModuleID { get; init; }
        public EntityState State { get; init; }
        public ILobby Lobby { get; init; }

        public static WorldEntity CreateWorldEntity(string? name, int moduleId, EntityState state, ILobby lobby)
        {
            return new WorldEntity(name, moduleId, state, lobby);
        }

        private WorldEntity(string? name, int moduleId, EntityState state, ILobby lobby) 
        {
            Name = name;
            Id = Guid.NewGuid();
            ModuleID = moduleId;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
            Lobby = lobby ?? throw new ArgumentNullException(nameof(lobby), "Lobby cannot be null.");
        }

        public void UpdateState(EntityState newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            State.Health = newState.Health;
            State.Position = new (newState.Position);
            State.Hunger = newState.Hunger;
            State.InteractionCooldownLeft = newState.InteractionCooldownLeft;

            if (State.Health <= 0)
            {
                Die(ModuleService.Instance);
            }
        }

        public void UpdateStateWithDelta(Position2D positionDelta, int healthDelta, int hungerDelta)
        {
            State.Position += positionDelta;
            State.Health += healthDelta;
            State.Hunger += hungerDelta;

            Module? module = ModuleService.Instance.GetModuleById(ModuleID);
            
            // Debug
            if (module != null && module.Type != EntityTypeEnum.Human)
            {
                Console.WriteLine($"{module.Name} -> new state: Health={State.Health}, Hunger={State.Hunger})");
            }

            if (State.Health <= 0)
            {
                Die(ModuleService.Instance);
            }
        }

        public void Die(IModuleService moduleService)
        {
            Module? module = moduleService.GetModuleById(ModuleID) ?? throw new ModuleNotFoundException($"Module with ID {ModuleID} not found for entity {Id}");
            this.State.Health = 0;
            this.State.Hunger = 0;
            this.State.Position = new Position2D(0,0);

            if (module.Type == EntityTypeEnum.Human)
            {
                //noop for now
                //Client side
            }
            else
            {
                Lobby.DestroyWorldEntity(this);
            }
        }

        public WorldEntityDTO ToDTO()
        {
            return new WorldEntityDTO (this.Name, this.Id, this.State.ToDTO(), this.ModuleID);
        }

    }
}
