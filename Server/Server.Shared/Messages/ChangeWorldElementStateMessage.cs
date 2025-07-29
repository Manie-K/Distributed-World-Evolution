using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class ChangeWorldElementStateMessage : IMessage
    {
        public IMessageType MessageType => IMessageType.ChangeWorldElementState;

        //TODO: add properties for world element state change
        public Guid GUID { get; set; }
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public int Health { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                message_type = MessageType,
                element_guid = GUID.ToString(),
                element_name = ElementName,
                element_type = ElementType,
                element_health = Health,
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}