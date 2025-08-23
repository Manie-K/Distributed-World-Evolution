namespace SharedLibrary
{
    public class InfoMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.InfoMessage;
        
        /// <summary>
        /// The message content.
        /// </summary>
        public string MessageContent { get; set; }

        public InfoMessage()
        {}

        public InfoMessage(object messageContent)
        {
            if (messageContent == null)
                throw new ArgumentNullException(nameof(messageContent), "Message content cannot be null.");

            MessageContent = messageContent.ToString();        
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
