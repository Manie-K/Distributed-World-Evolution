using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class ChangeWorldElementStateMessage : MessageBase
    {
        public override MessageTypeEnum MessageType => MessageTypeEnum.ChangeWorldElementState;

        //TODO: add properties for world element state change
        public Guid ElementGUID { get; set; }
        public string ElementName { get; set; }
        public string ElementType { get; set; }
        public int ElementHealth { get; set; }


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                ElementGUID = ElementGUID.ToString(),
                ElementName = ElementName,
                ElementType = ElementType,
                ElementHealth = ElementHealth,
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}