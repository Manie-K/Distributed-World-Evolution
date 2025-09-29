namespace SharedLibrary.Messages
{
    public class InfoMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.InfoMessage;

        public InfoMessageTypeEnum InfoMessageType { get; init; }
        public string MessageContent { get; init; }

        public InfoMessage(InfoMessageTypeEnum infoMessageType ,string messageContent)
        {
            InfoMessageType = infoMessageType;
            MessageContent = messageContent;        
        }

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
