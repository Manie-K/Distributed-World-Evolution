using SharedLibrary.Messages;
using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class RoleMessage : MessageBase
    {
        public RoleEnum Role { get; init; }

        public override MessageTypeEnum MessageType => MessageTypeEnum.RoleMessage;

        public RoleMessage(RoleEnum role)
        {
            Role = role;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                Role = Role      
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
