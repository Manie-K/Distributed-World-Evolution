namespace SharedLibrary.Messages
{
    public class ErrorMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.ErrorMessage;

        public string MessageContent { get; init; }

        public ErrorMessage(string messageContent)
        {
            MessageContent = messageContent;
        }

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
