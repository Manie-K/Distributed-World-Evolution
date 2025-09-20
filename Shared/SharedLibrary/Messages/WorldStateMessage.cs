using System.Text.Json;
using SharedLibrary;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    public class WorldStateMessage : MessageBase
    {
        public IEnumerable<WorldEntityDTO> Entities { get; init; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.WorldState;

        public WorldStateMessage(IEnumerable<WorldEntityDTO> entities)
        {
            Entities = entities;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Entities = Entities
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}