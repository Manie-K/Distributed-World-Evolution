using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
    /// <summary>
    /// Class representing the state of an entity in the game world.
    /// </summary>
    public class EntityState
    {
        /// <summary>
        /// The position of the entity in the game world.
        /// </summary>
        public Position2D Position { get; set; }
        /// <summary>
        /// The health of the entity.
        /// </summary>
        public int Health { get; set; }

        /// <summary>
        /// The hunger of the entity.
        /// </summary>
        public int Hunger { get; set; }

        /// <summary>
        /// The number of cycles left of interaction cooldown.
        /// </summary>
        public int InteractionCooldownLeft { get; set; }

        /// <summary>
        /// The name of the last interaction performed by the entity - used for displaying animations on client side.
        /// </summary>
        public string LastInteractionName { get; set; } = String.Empty;

        /// <summary>
        /// Last movement vector of the entity - used in client.
        /// </summary>
        public Position2D LastMovementVector { get; set; } = new Position2D(0, 0);
        //We don't persist this value, it's only for runtime use. Reset to (0,0) won't break anything.

        /// <summary>
        /// Last attacked entity ID - used for combat mechanics.
        /// </summary>
        public Guid LastAttackedEntityId { get; set; } = Guid.Empty; //Skip in DTOs as well.

        /// <summary>
        /// Creates a new instance of EntityState.
        /// </summary>
        /// <param name="position"> Position of the entity. </param>
        /// <param name="health"> Health of the entity. </param>
        /// <param name="hunger"> Hunger level of the entity. </param>
        /// <param name="interactionFramesLeft"> Number of frames left for interaction cooldown. </param>
        public EntityState(Position2D position, int health, int hunger, int interactionFramesLeft = 0)
        {
            Position = new Position2D(position.X, position.Y);
            Health = health;
            Hunger = hunger;
            InteractionCooldownLeft = interactionFramesLeft;
        }

        /// <summary>
        /// Creates a new instance of EntityState by copying another instance values.
        /// </summary>
        /// <param name="other"> Other EntityState to copy values from. </param>
        public EntityState(EntityState other)
        {
            Position = new Position2D(other.Position.X, other.Position.Y);
            Health = other.Health;
            Hunger = other.Hunger;
            InteractionCooldownLeft = other.InteractionCooldownLeft;
        }

        /// <summary>
        /// Creates a new instance of EntityState from a DTO.
        /// </summary>
        /// <param name="dto"> DTO to create the EntityState from. </param>
        public EntityState(EntityStateDTO dto)
        {
            Position = new Position2D(dto.Position.X, dto.Position.Y);
            Health = dto.Health;
            Hunger = dto.Hunger;
        }

        /// <summary>
        /// Converts the EntityState to a DTO.
        /// </summary>
        /// <returns> DTO representing the EntityState. </returns>
        public EntityStateDTO ToDTO()
        {
            return new EntityStateDTO(Position, Health, Hunger, LastInteractionName);
        }

        /// <summary>
        /// Compares the EntityState with a DTO.
        /// </summary>
        /// <param name="dto"> DTO to compare with. </param>
        /// <returns> True if the EntityState is equal to the DTO; otherwise, false. </returns>
        public bool EqualsDto(EntityStateDTO? dto)
        {
            if (dto == null)
            {
                return false;
            }

            return Position.Equals(dto.Position)
                && Health == dto.Health
                && Hunger == dto.Hunger
                && LastInteractionName == dto.LastInteractionName;
        }

    }

}