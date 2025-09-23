using System.Numerics;
using SharedLibrary;

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