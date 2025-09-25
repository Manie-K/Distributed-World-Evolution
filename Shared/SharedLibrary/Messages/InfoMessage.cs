namespace SharedLibrary.Messages
{
    public class InfoMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.InfoMessage;
        
        /// <summary>
        /// The message content.
        /// </summary>
        public string MessageContent { get; init; }

        public InfoMessage(string messageContent)
        {
            MessageContent = messageContent;        
        }

        /// <summary>
        /// Builds the JSON representation of the message.
        /// </summary>
        /// <returns>A JSON string representing the message.</returns>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                MessageContent = MessageContent
            };
            return System.Text.Json.JsonSerializer.Serialize(payload);
        }
    }
}
