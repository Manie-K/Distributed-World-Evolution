using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    //TODO: Change after entity is fully implemented
    public class EntityStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.EntityState;

        public WorldEntityDTO Entity { get; init; }

        public EntityStateMessage(WorldEntityDTO entity) 
        {
            Entity = entity;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Entity = this.Entity
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}