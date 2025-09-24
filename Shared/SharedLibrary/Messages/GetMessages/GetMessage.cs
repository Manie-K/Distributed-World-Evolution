using System.Text.Json;

namespace SharedLibrary.Messages
{
    public class GetMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.GetMessage;

        public GetMessageTypeEnum GetMessageType { get; init; }

        public GetMessage(GetMessageTypeEnum getMessageType)
        {
            GetMessageType = getMessageType;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                GetMessageType = this.GetMessageType
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
