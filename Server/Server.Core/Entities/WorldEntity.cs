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

            if(State.Health <= 0)
            {
                Die(ModuleService.Instance);
            }
        }

        public void UpdateStateWithDelta(Position2D positionDelta, int healthDelta, int hungerDelta)
        {
            //TODO: change and add checks
            State.Position = positionDelta;
            State.Health = healthDelta;
            State.Hunger = hungerDelta;
        }

        public void Die(IModuleService moduleService)
        {
            Module? module = moduleService.GetModuleById(ModuleID);
            if (module == null)
            {
                throw new Exception($"Module with ID {ModuleID} not found for entity {Id}");
            }

            if (module.Type == EntityTypeEnum.Human)
            {
                //@EVERYONE, What do we do here?
                //noop for now
                //Client side?
            }
            else
            {
                // Remove entity from lobby
                Lobby.DestroyWorldEntity(this);
            }
        }

        public WorldEntityDTO ToDTO()
        {
            return new WorldEntityDTO (this.Name, this.Id, this.State.ToDTO(), this.ModuleID);
        }

    }
}
