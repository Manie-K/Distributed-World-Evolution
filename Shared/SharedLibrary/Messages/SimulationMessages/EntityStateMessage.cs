using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    public class EntityStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.EntityState;

        public WorldEntityDTO HumanEntity { get; init; }
        public WorldEntityDTO? OtherEntity { get; init; }

        public EntityStateMessage(WorldEntityDTO human, WorldEntityDTO? other) 
        {
            HumanEntity = human;
            OtherEntity = other;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                HumanEntity = this.HumanEntity,
                OtherEntity = this.OtherEntity
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}