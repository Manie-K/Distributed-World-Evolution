using System.Text.Json;
using Server.Shared.Entities;

namespace Server.Shared.Messages
{
    public class UpdateWorldEntityStateMessage : MessageBase
    {
        //TODO: @FranciszekGwarek make sure the deserialization works correctly

        public WorldEntity Entity { get; private set; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.UpdateWorldEntityState;
        public UpdateWorldEntityStateMessage(WorldEntity entity) 
        {
            Entity = entity;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Entity = this.Entity
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}