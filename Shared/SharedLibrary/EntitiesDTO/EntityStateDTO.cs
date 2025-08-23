using System.Numerics;

namespace SharedLibrary
{
    public class EntityStateDTO
    {
        public Vector2 Position { get; set; }

        public EntityStateDTO(Vector2 position)
        {
            Position = position;
        }
    }
}