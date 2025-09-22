using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Logging;

namespace SharedLibrary.Messages
{
    public class LogMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.LogMessage;
        public OnLogEventArgs OnLogEventArgs { get; init; }
        public int SenderID { get; init; }

        public LogMessage(OnLogEventArgs onLogEventArgs, int senderID)
        {
            OnLogEventArgs = onLogEventArgs;
            SenderID = senderID;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                OnLogEventArgs = OnLogEventArgs,
                SenderID = SenderID
            };

            return System.Text.Json.JsonSerializer.Serialize(payload);
        }
    }
}
