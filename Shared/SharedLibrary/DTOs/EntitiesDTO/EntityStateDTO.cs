using SharedLibrary.Helpers;

namespace SharedLibrary.DTOs.EntitiesDTO
{
    public class EntityStateDTO
    {
        public Position2D Position { get; set; }
        public int Health { get; set; }
        public int Hunger { get; set; }
        public int InteractionFramesLeft { get; set; }
        public string LastInteractionName{ get; set; }

        public EntityStateDTO(Position2D position, int health, int hunger, int interactionFramesLeft, string lastInteractionName)
        {
            Position = position;
            Health = health;
            Hunger = hunger;
            LastInteractionName = lastInteractionName;
        }
    }
}