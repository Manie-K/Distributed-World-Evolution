using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Server.Core
{
    public class WorldEntity
    {
        public Guid Id { get; init; }
        public string? Name { get; init; }
        public int ModuleID { get; init; }
        public EntityState State { get; init; }

        public static WorldEntity CreateWorldEntity(string? name, int moduleId, EntityState state)
        {
            return new WorldEntity(name, moduleId, state);
        }

        private WorldEntity(string? name, int moduleId, EntityState state) 
        {
            Name = name;
            Id = Guid.NewGuid();
            ModuleID = moduleId;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
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
            State.InteractionFramesLeft = newState.InteractionFramesLeft;
        }

        public void Die()
        {
            // VERY IMPORTANT TODO
            // noop for now
        }

        public WorldEntityDTO ToDTO()
        {
            return new WorldEntityDTO (this.Name, this.Id, this.State.ToDTO(), this.ModuleID);
        }

    }
}
