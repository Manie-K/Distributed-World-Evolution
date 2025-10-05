using System.Text.Json;
using SharedLibrary.DTOs.EntitiesDTO;

namespace SharedLibrary.Messages
{
    public class UserInteractionMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.UserInteraction;

        public WorldEntityDTO HumanEntity { get; init; }
        public WorldEntityDTO? OtherEntity { get; init; }

        public UserInteractionMessage(WorldEntityDTO human, WorldEntityDTO? other) 
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