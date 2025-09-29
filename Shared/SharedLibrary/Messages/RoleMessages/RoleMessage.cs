using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class RoleMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.RoleMessage;

        public RoleEnum Role { get; init; }

        public RoleMessage(RoleEnum role)
        {
            Role = role;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                Role = this.Role      
            };

            return JsonSerializer.Serialize(payload);
        }

    }
}
