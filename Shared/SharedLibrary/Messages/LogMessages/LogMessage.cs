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
        /// Log arguments.
        /// </summary>
        public Log Log { get; init; }
        /// <summary>
        /// Sender ID of the log message.
        /// </summary>
        public int SenderID { get; init; }
        /// <summary>
        /// Constructor for LogMessage.
        /// </summary>
        public LogMessage(Log log, int senderID)
        {
            Log = log;
            SenderID = senderID;
        }
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                Log = Log,
                SenderID = SenderID
            };

            return JsonSerializer.Serialize(payload);
        }
    }
}
