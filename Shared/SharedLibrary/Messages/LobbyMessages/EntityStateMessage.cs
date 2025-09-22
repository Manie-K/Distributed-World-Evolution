using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    public class EntityStateMessage : MessageBase
    {
        public WorldEntityDTO Entity { get; init; }

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