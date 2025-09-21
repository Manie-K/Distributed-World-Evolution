using System.Numerics;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Server.Core
{
    public class EntityState
    {
        public Vector2 Position { get; set; }

        public EntityStateDTO ToDTO()
        {
            return new EntityStateDTO(Position);
        }

    }
}