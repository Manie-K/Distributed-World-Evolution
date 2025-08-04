using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Shared.Messages
{
    public class JoinLobbyMessage : MessageBase
    {
        public MessageType MessageType => MessageType.JoinLobby;

        //TODO: add properties for lobby joining
        public int LobbyID { get; set; }


        public override string BuildJson()
        {
            var payload = new
            {
                MessageType = MessageType,
                LobbyID = LobbyID
                //TODO: add other properties as needed
            };

            return JsonSerializer.Serialize(payload);
        }
    }

}
