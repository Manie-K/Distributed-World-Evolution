using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
    public class EntityState
    {
        public Position2D Position { get; set; }
        public int Health { get; set; }

        /// <summary>
        /// 100 means not hungry at all, 0 means starving
        /// In animal and humans, it decreases over time, and eating increases it
        /// In plants, this value isn't changing, and eating this plant grants this value to the eater hunger
        /// </summary>
        public int Hunger { get; set; }
        public int InteractionFramesLeft { get; set; }

        public string LastInteractionName { get; set; } = String.Empty;

        //We don't persist this value, it's only for runtime use. Reset to (0,0) won't break anything.
        public Position2D LastMovementVector { get; set; } = new Position2D(0, 0);

        public EntityState(Position2D position, int health, int hunger, int interactionFramesLeft = 0)
        {
            Position = new Position2D(position.X, position.Y);
            Health = health;
            Hunger = hunger;
            InteractionFramesLeft = interactionFramesLeft;
        }

        public EntityState(EntityState other)
        {
            Position = new Position2D(other.Position.X, other.Position.Y);
            Health = other.Health;
            Hunger = other.Hunger;
            InteractionFramesLeft = other.InteractionFramesLeft;
        }

        public EntityState(EntityStateDTO dto)
        {
            Position = new Position2D(dto.Position.X, dto.Position.Y);
            Health = dto.Health;
            Hunger = dto.Hunger;
            InteractionFramesLeft = dto.InteractionFramesLeft;
        }

        public EntityStateDTO ToDTO()
        {
            return new EntityStateDTO(Position, Health, Hunger, InteractionFramesLeft, LastInteractionName);
        }

    }
}