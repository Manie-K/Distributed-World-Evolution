using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Server.Core
{
    public class WorldEntity
    {
        public Guid Id { get; init; }
        public int ModuleID { get; init; }
        public EntityState State { get; init; }

        public static WorldEntity CreateWorldEntity(int moduleId)
        {
            return new WorldEntity(moduleId, null);
        }

        private WorldEntity(int moduleId, EntityState state) 
        {
            Id = Guid.NewGuid();
            ModuleID = moduleId;
            State = state ?? throw new ArgumentNullException(nameof(state), "State cannot be null.");
        }

        /*
        public void UpdateStateWithDTO(EntityStateDTO newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException(nameof(newState), "New state cannot be null.");
            }

            //New to iterate on all props
            State.Position = newState.Position;
        }*/

        public WorldEntityDTO ToDTO()
        {
            throw new NotImplementedException();
        }
    }
}
