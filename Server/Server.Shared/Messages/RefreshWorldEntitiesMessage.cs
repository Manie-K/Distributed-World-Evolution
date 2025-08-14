using System.Text.Json;
using Server.Shared.Entities;

namespace Server.Shared.Messages
{
    public class RefreshWorldEntitiesMessage : MessageBase
    {
        //TODO: @FranciszekGwarek make sure the deserialization works correctly
        public IEnumerable<WorldEntity> Entities { get; private set; }


        public override MessageTypeEnum MessageType => MessageTypeEnum.RefreshWorldEntities;

        public RefreshWorldEntitiesMessage(IEnumerable<WorldEntity> entities)
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