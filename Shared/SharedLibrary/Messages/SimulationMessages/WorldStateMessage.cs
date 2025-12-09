using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains the updated state of the world's entities.
    /// </summary>
    public class WorldStateMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.WorldState;

        /// <summary>
        /// Updated entities in the world.
        /// </summary>
        public IEnumerable<WorldEntityDTO> UpdatedEntities { get; init; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="updatedEntities"> Updated entities in the world. </param>
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