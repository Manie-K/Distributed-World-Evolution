using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
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

        public Guid LastAttackedEntityId { get; set; } = Guid.Empty; //Skip in DTOs as well.

        /// <summary>
        /// Creates a new instance of EntityState.
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="health">Health</param>
        /// <param name="hunger">Hunger</param>
        /// <param name="interactionFramesLeft">Cooldown left</param>
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
        /// <param name="other"></param>
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
        /// <param name="dto">Data transfer object</param>
        public EntityState(EntityStateDTO dto)
        {
            Position = new Position2D(dto.Position.X, dto.Position.Y);
            Health = dto.Health;
            Hunger = dto.Hunger;
            InteractionCooldownLeft = dto.InteractionFramesLeft;
        }

        /// <summary>
        /// Converts the EntityState to a DTO.
        /// </summary>
        /// <returns></returns>
        public EntityStateDTO ToDTO()
        {
            return new EntityStateDTO(Position, Health, Hunger, InteractionCooldownLeft, LastInteractionName);
        }

        /// <summary>
        /// Compares the EntityState with a DTO.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public bool EqualsDto(EntityStateDTO? dto)
        {
            if (dto == null)
            {
                return false;
            }

            return Position.Equals(dto.Position)
                && Health == dto.Health
                && Hunger == dto.Hunger
                && InteractionCooldownLeft == dto.InteractionFramesLeft
                && LastInteractionName == dto.LastInteractionName;
        }

    }
}