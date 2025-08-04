using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class ChangeUserStateMessage : MessageBase
    {
        public MessageType MessageType => MessageType.ChangeUserState;

        //TODO: add properties for user state change
        public Guid UserGUID { get; set; }
        public string UserName { get; set; }
        public int UserHealth { get; set; }


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                UserGUID = UserGUID.ToString(),
                UserName = UserName,
                UserHealth = UserHealth,
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}