using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Messages
{
    public class LogMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.LogMessage;
        public OnLogEventArgs OnLogEventArgs { get; private set; }
        public int SenderID { get; private set; }

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
