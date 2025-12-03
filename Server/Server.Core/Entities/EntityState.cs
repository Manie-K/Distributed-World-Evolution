using SharedLibrary.DTOs.EntitiesDTO;
using SharedLibrary.Helpers;

namespace Server.Core
{
    public class EntityState
    {
        /// <summary>
        /// The position of the entity in the game world.
        /// </summary>
        public Position2D Position 
        { 
            get 
            { 
                return _position;
            } 
            set 
            {
                if (value.X < 0 || value.X >= 200 || value.Y < 0 || value.Y > 200)
                {
                    Console.WriteLine($"X: {value.X}, Y: {value.Y}");
                    Console.WriteLine(Environment.StackTrace);
                    throw new ArgumentOutOfRangeException(nameof(value), "Position is out of bounds.");
                }
                _position = value;
            } 
        }

        private Position2D _position;
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
            if (position.X < 0 || position.X >= 200 || position.Y < 0 || position.Y > 200)
            {
                Console.WriteLine($"X: {position.X}, Y: {position.Y} - EntityState(...........)");
                Console.WriteLine(Environment.StackTrace);
                throw new ArgumentOutOfRangeException(nameof(position), "Position is out of bounds.");
            }
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
            if (other.Position.X < 0 || other.Position.X >= 200 || other.Position.Y < 0 || other.Position.Y > 200)
            {
                Console.WriteLine($"X: {other.Position.X}, Y: {other.Position.Y} - EntityState(EntityState other)");
                Console.WriteLine(Environment.StackTrace);
                throw new ArgumentOutOfRangeException(nameof(other.Position), "Position is out of bounds.");
            }
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