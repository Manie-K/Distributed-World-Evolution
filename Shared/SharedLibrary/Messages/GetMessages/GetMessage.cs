using System.Text.Json;

namespace SharedLibrary.Messages
{
    /// <summary>
    /// Message used to request specific information from the server.
    /// </summary>
    public class GetMessage : MessageBase
    {
        /// <inheritdoc/>
        public override MessageTypeEnum MessageType => MessageTypeEnum.GetMessage;

        /// <summary>
        /// Type of information being requested.
        /// </summary>
        public GetMessageTypeEnum GetMessageType { get; init; }

        /// <summary>
        /// Constructor for GetMessage.
        /// </summary>
        /// <param name="getMessageType"> Type of information being requested. </param>
        public GetMessage(GetMessageTypeEnum getMessageType)
        {
            GetMessageType = getMessageType;
        }
        
        /// <inheritdoc/>
        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = this.MessageType,
                GetMessageType = this.GetMessageType
            };

            return JsonSerializer.Serialize(payload);
        }

    }

}
