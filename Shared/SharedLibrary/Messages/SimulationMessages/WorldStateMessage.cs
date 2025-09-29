using System.Text.Json;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    //TODO: Change after world state is fully implemented
    public class WorldStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.WorldState;

        public IEnumerable<WorldEntityDTO> Entities { get; init; }

        public WorldStateMessage(IEnumerable<WorldEntityDTO> entities)
        {
            Entities = entities;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Entities = this.Entities
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}