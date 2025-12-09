using SharedLibrary.Helpers;

namespace SharedLibrary.DTOs.EntitiesDTO
{
    /// <summary>
    /// DTO representing the state of an entity.
    /// </summary>
    public class EntityStateDTO
    {
        /// <summary>
        /// Position of the entity.
        /// </summary>
        public Position2D Position { get; set; }

        /// <summary>
        /// Health of the entity.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// Huner level of the entity.
        /// </summary>
        public int Hunger { get; set; }

        /// <summary>
        /// Last interaction name of the entity.
        /// </summary>  
        public string LastInteractionName{ get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="position"> Position of the entity. </param>  
        /// <param name="health"> Health of the entity. </param>
        /// <param name="hunger"> Hunger level of the entity. </param>
        /// <param name="lastInteractionName"> Last interaction name of the entity. </param>
        public EntityStateDTO(Position2D position, int health, int hunger, string lastInteractionName)
        {
            Position = position;
            Health = health;
            Hunger = hunger;
            LastInteractionName = lastInteractionName;
        }

    }

}