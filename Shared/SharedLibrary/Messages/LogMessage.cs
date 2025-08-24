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
        public object? Sender { get; private set; }

        public LogMessage(OnLogEventArgs onLogEventArgs, object? sender)
        {
            OnLogEventArgs = onLogEventArgs;
            Sender = sender;
        }

        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                OnLogEventArgs = OnLogEventArgs,
                Sender = Sender?.ToString()
            };

            return System.Text.Json.JsonSerializer.Serialize(payload);
        }
    }
}
