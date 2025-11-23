using System.Text.Json;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains the state of the world.
    /// </summary>
    public class WorldStateMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.WorldState;
        /// <summary>
        /// World entities.
        /// </summary>
        public IEnumerable<WorldEntityDTO> UpdatedEntities { get; init; }
        /// <summary>
        /// Constructor for WorldStateMessage.
        /// </summary>
        public WorldStateMessage(IEnumerable<WorldEntityDTO> updatedEntities)
        {
            UpdatedEntities = updatedEntities;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                UpdatedEntities = this.UpdatedEntities
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}