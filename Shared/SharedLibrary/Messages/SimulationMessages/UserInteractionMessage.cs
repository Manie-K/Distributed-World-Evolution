using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Represents a user interaction event.
    /// </summary>
    public class UserInteractionMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.UserInteraction;
        /// <summary>
        /// Human entity involved in the interaction.
        /// </summary>
        public WorldEntityDTO HumanEntity { get; init; }
        /// <summary>
        /// Other entity involved in the interaction, if any.
        /// </summary>
        public WorldEntityDTO? OtherEntity { get; init; }
        /// <summary>
        /// Constructor for UserInteractionMessage.
        /// </summary>
        public UserInteractionMessage(WorldEntityDTO humanEntity, WorldEntityDTO? otherEntity) 
        {
            HumanEntity = humanEntity;
            OtherEntity = otherEntity;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                HumanEntity = this.HumanEntity,
                OtherEntity = this.OtherEntity
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}