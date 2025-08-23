using System.Text.Json;
using SharedLibrary;

namespace SharedLibrary
{
    public class EntityStateMessage : MessageBase
    {
        //TODO: @FranciszekGwarek make sure the deserialization works correctly

        public WorldEntityDTO Entity { get; private set; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.EntityState;
        public EntityStateMessage(WorldEntityDTO entity) 
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