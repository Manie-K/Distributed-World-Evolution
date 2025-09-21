using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                MessageType = MessageType,
                GetMessageType = GetMessageType
            };

            return System.Text.Json.JsonSerializer.Serialize(payload);
        }
    }
}
