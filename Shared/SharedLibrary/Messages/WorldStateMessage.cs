using System.Text.Json;
using SharedLibrary;

namespace SharedLibrary
{
    public class WorldStateMessage : MessageBase
    {
        //TODO: @FranciszekGwarek make sure the deserialization works correctly
        public IEnumerable<WorldEntityDTO> Entities { get; private set; }


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
                Entities = this.Entities
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}