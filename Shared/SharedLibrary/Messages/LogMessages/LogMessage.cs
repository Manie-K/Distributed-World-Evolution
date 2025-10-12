using SharedLibrary.Logging;
using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Contains log event information.
    /// </summary>
    public class LogMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.LogMessage;
        /// <summary>
        /// Log event arguments.
        /// </summary>
        public OnLogEventArgs OnLogEventArgs { get; init; }
        /// <summary>
        /// Sender ID of the log message.
        /// </summary>
        public int SenderID { get; init; }

        /// <summary>
        /// Constructor for LogMessage.
        /// </summary>
        public LogMessage(OnLogEventArgs onLogEventArgs, int senderID)
        {
            OnLogEventArgs = onLogEventArgs;
            SenderID = senderID;
        }

        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                OnLogEventArgs = OnLogEventArgs,
                SenderID = SenderID
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
