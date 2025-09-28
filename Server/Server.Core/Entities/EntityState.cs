using System.Numerics;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace Server.Core
{
    public class EntityState
    {
        public Vector2 Position { get; set; }
        public int Health { get; set; }

        /// <summary>
        /// 100 means not hungry at all, 0 means starving
        /// In animal and humans, it decreases over time, and eating increases it
        /// In plants, this value isn't changing, and eating this plant grants this value to the eater hunger
        /// </summary>
        public int Hunger { get; set; }
        public int InteractionFramesLeft { get; set; }


        public EntityStateDTO ToDTO()
        {
            return new EntityStateDTO(Position, Health, Hunger, InteractionFramesLeft);
        }

    }
}