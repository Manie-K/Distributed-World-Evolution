namespace SharedLibrary.Messages
{
    /// <summary>
    /// Message used to convey informational messages, warnings, or errors.
    /// </summary>
    public class InfoMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.InfoMessage;
        
        /// <summary>
        /// Type of the info message.
        /// </summary>
        public InfoMessageTypeEnum InfoMessageType { get; init; }
        
        /// <summary>
        /// Optional content of the message.
        /// </summary>
        public string? MessageContent { get; init; }

        /// <summary>
        /// Constructor .
        /// </summary>
        /// <param name="infoMessageType"> Type of the info message. </param>
        /// <param name="messageContent"> Optional content of the message. </param>
        public InfoMessage(InfoMessageTypeEnum infoMessageType, string? messageContent)
        {
            InfoMessageType = infoMessageType;
            MessageContent = messageContent;        
        }
        
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                InfoMessageType = this.InfoMessageType,
                MessageContent = this.MessageContent
            };

            return System.Text.Json.JsonSerializer.Serialize(payload);
        }

    }

}
