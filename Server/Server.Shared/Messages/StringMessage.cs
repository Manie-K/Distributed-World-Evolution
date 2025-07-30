using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class StringMessage : IMessage
    {
        public IMessageType MessageType => IMessageType.String;
        /// <summary>
        /// The message content.
        /// </summary>
        public string MessageContent { get; set; }

        public StringMessage()
        {}

        public StringMessage(string messageContent)
        {
            MessageContent = messageContent ?? throw new ArgumentNullException(nameof(messageContent), "String message content cannot be null.");
        }

        /// <summary>
        /// Builds the JSON representation of the message.
        /// </summary>
        /// <returns>A JSON string representing the message.</returns>
        public string BuildJson()
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
