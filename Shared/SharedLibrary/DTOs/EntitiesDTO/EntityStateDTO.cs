using System.Numerics;

namespace SharedLibrary.DTOs.EntitiesDTO
{
    public class EntityStateDTO
    {
        public Vector2 Position { get; set; }
        public int Health { get; set; }
        public int Hunger { get; set; }

        public EntityStateDTO(Vector2 position, int health, int hunger)
        {
            Position = position;
            Health = health;
            Hunger = hunger;
        }
    }
}