using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class ChangeWorldElementStateMessage : IMessageBuilder
    {
        public string MessageType => "ChangeWorldElementState";

        //TODO: add properties for world element state change
        public Guid GUID { get; set; }
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public int Health { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                type = MessageType,
                guid = GUID.ToString(),
                elementaName = ElementName,
                elementType = ElementType,
                health = Health,
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}