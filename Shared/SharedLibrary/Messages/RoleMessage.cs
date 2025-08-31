using SharedLibrary.Messages;
using System.Text.Json;

namespace SharedLibrary
{
    public class RoleMessage : MessageBase
    {

        public RoleEnum Role { get; private set; }

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
