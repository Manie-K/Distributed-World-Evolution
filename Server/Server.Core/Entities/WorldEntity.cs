using System;
using Server.Core.Modules;
using SharedLibrary;

namespace Server.Core
{
    public class WorldEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public EntityState State { get; private set; }
        public Module Module { get; init; }
        public int ModuleID { get; init; }

        public static WorldEntity CreateWorldEntity(string? name, string? description, Module module ,EntityState? initialState)
        {
            return new WorldEntity(name ?? "Default name", description ?? "Default description", module, initialState);
        }

        private WorldEntity(string name, string description, Module module, EntityState state) 
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null.");
            Description = description ?? throw new ArgumentNullException(nameof(description), "Description cannot be null.");
            CreatedAt = DateTime.UtcNow;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
            //Module = module ?? throw new ArgumentNullException(nameof(module), "Module cannot be null.");
            ModuleID = module.ID;
        }

        public void UpdateStateWithDTO(EntityStateDTO newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            //New to iterate on all props
            State.Position = newState.Position;
        }

        public WorldEntityDTO ToDTO()
        {
            return new WorldEntityDTO(Id, State.ToDTO());
        }
    }
}
