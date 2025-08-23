using System;
namespace SharedLibrary
{
    public class WorldEntityDTO
    {
        public Guid Id { get; private set; }
        public EntityStateDTO State { get; private set; }

        public WorldEntityDTO(Guid id, EntityStateDTO state)
        {
            Id = id;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
        }
    }
}
