using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class ChangeUserStateMessage : IMessageBuilder
    {
        public string MessageType => "ChangeUserState";

        //TODO: add properties for user state change
        public Guid GUID { get; set; }
        public string UserName { get; set; }
        public int Health { get; set; }


        public string BuildJson()
        {
            var payload = new
            {
                type = MessageType,
                guid = GUID.ToString(),
                userName = UserName,
                health = Health,
                // Add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}