using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class DefaultMessage : MessageBase
    {
        public MessageType MessageType => MessageType.DefaultMessage;
        /// <summary>
        /// The message content.
        /// </summary>
        public string MessageContent { get; set; }

        public DefaultMessage()
        {}

        public DefaultMessage(object messageContent)
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
